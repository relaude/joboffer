using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Enum;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class JobOfferDocumentService : IJobOfferDocumentService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IJOLetterService _joLetterService;
        private readonly IJOFileService _joFileService;
        private readonly IHtmlToPDFServices _htmlToPdfServices;
        private readonly IProtectPDFService _protectPdfService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<JobOfferDocumentService> _logger;

        public JobOfferDocumentService(IDbContextFactory<JobOfferDbContext> dbContext,
            IJOLetterService joLetterService,
            IJOFileService joFileService,
            IHtmlToPDFServices htmlToPdfServices,
            IProtectPDFService protectPdfService,
            IWebHostEnvironment environment,
            ILogger<JobOfferDocumentService> logger)
        {
            _dbContext = dbContext;
            _joLetterService = joLetterService;
            _joFileService = joFileService;
            _htmlToPdfServices = htmlToPdfServices;
            _protectPdfService = protectPdfService;
            _environment = environment;
            _logger = logger;
        }

        public async Task SaveJobOfferEmailAsync(int jobOfferId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(jobOfferId);

            var jobOffer = await _joLetterService.GetJobOffer(jobOfferId)
                ?? throw new InvalidOperationException($"Job offer {jobOfferId} was not found.");
            if (jobOffer.CandidateId is not int candidateId || string.IsNullOrWhiteSpace(jobOffer.RefNum))
                throw new InvalidOperationException("A candidate and reference number are required to save the email.");

            var candidate = await _joLetterService.GetVwDboxCandidate(candidateId);
            if (candidate is null || string.IsNullOrWhiteSpace(candidate.EmailAddress))
                throw new InvalidOperationException("The candidate must have an email address.");

            var template = await _joLetterService.EditedEmailTemplate(jobOfferId, 1);
            if (template.Id == 0 || string.IsNullOrWhiteSpace(template.EmailSubject)
                || string.IsNullOrWhiteSpace(template.EmailMessage))
                throw new InvalidOperationException("Email template 1 must have a subject and message.");

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                throw new InvalidOperationException("The web root directory is not configured.");

            await using var context = await _dbContext.CreateDbContextAsync();
            // Only drafts may be refreshed; submitted or approved emails remain intact.
            var jobOfferEmail = await context.JobOfferHasEmail.AsNoTracking()
                .Where(email => email.JobOfferId == jobOfferId && email.StatusId == (int)EnumJOEmailStatus.Draft)
                .OrderByDescending(email => email.Id)
                .FirstOrDefaultAsync()
                ?? new JobOfferHasEmail { JobOfferId = jobOfferId };

            var candidateFileName = string.Concat((candidate.CandidateName ?? string.Empty)
                .Where(character => !char.IsWhiteSpace(character)
                    && !Path.GetInvalidFileNameChars().Contains(character)));
            if (string.IsNullOrWhiteSpace(candidateFileName))
                throw new InvalidOperationException("The candidate must have a valid name for document filenames.");

            var existingDocuments = await context.JobOfferDocuments.AsNoTracking()
                .Where(document => document.JobOfferId == jobOfferId && document.CandidateId == candidateId)
                .ToListAsync();
            var attachments = new List<(string Name, byte[] Content, int DocumentType, int SalaryOptionId)>();
            // Prepare all files before saving the draft, so rendering failures leave it unchanged.
            if (!existingDocuments.Any(document => document.DocumentType == (int)EnumJODocumentType.Benefits && document.SalaryOptionId == 0))
                attachments.Add(($"{candidateFileName}-Benefits.pdf", await File.ReadAllBytesAsync(
                    Path.Combine(_environment.WebRootPath, "docs", "benefits.pdf")), 2, 0));

            var options = await _joLetterService.GetJOCompanyCompensation(jobOfferId);
            foreach (var option in options)
            {
                if (option.ProposedSalary is not decimal salary)
                    throw new InvalidOperationException($"Compensation option {option.OptionNumber} has no proposed salary.");

                var fileName = $"{candidateFileName}-Option-{option.OptionNumber}.pdf";
                if (existingDocuments.Any(document => document.DocumentType == (int)EnumJODocumentType.JOLetter && document.SalaryOptionId == option.Id))
                    continue;

                // Load fresh items for each option because placeholder replacement mutates them.
                var letterItems = await _joLetterService.GetJOItemLetter(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());

                decimal monthlyRiceAllowanace = await GetMonthlyRiceAllowance(jobOfferId);
                decimal dailyTranspoAllowance = await GetDailyTranspoAllowance(jobOfferId);

                _joLetterService.UpdateItemLetterPlaceHolder(letterItems, candidate, salary, monthlyRiceAllowanace, dailyTranspoAllowance);

                var letterBody = string.Concat(letterItems
                    .Where(item => !string.IsNullOrWhiteSpace(item.MessageBody))
                    .Select(item => item.MessageBody));
                if (string.IsNullOrWhiteSpace(letterBody))
                    throw new InvalidOperationException($"No letter content is available for option {option.OptionNumber}.");

                attachments.Add((fileName, await _htmlToPdfServices.GeneratePdfAsync(CreateLetterHtml(letterBody)), 1, option.Id));
                if (string.IsNullOrWhiteSpace(candidate.DboxRefNum)
                    || string.IsNullOrWhiteSpace(candidate.CandidateName))
                    throw new InvalidOperationException("The candidate reference number and name are required for the PDF password.");

                // CandidateName uses given names followed by the last name.
                var lastName = candidate.CandidateName.Split((char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries)[^1];
                var userPassword = candidate.DboxRefNum.Trim() + lastName;
                attachments[^1] = (fileName, _protectPdfService.ProtectPdf(attachments[^1].Content, userPassword: userPassword), 1, option.Id);
            }

            jobOfferEmail.CandidateId = candidateId;
            jobOfferEmail.ToRecipient = candidate.EmailAddress;
            jobOfferEmail.CCRecipient = template.CCRecipient;
            jobOfferEmail.Subject = template.EmailSubject;
            jobOfferEmail.EmailMessage = template.EmailMessage;

            if (jobOfferEmail.Id == 0)
                jobOfferEmail.Id = await _joLetterService.SaveDraftJobOfferHasEmail(jobOfferEmail);
            else
                await _joLetterService.UpdateJobOfferHasEmail(jobOfferEmail);

            if (attachments.Count == 0)
                return;

            var savedPaths = new List<string>();
            var records = new List<JobOfferDocuments>();
            try
            {
                foreach (var attachment in attachments)
                {
                    var path = await _joFileService.SaveJobOfferFileAsync(attachment.Content, jobOffer.RefNum,
                        $"{Guid.NewGuid():N}_{attachment.Name}");
                    savedPaths.Add(path);
                    records.Add(new JobOfferDocuments
                    {
                        JobOfferId = jobOfferId,
                        CandidateId = candidateId,
                        DocumentType = attachment.DocumentType,
                        SalaryOptionId = attachment.SalaryOptionId,
                        FileName = attachment.Name,
                        RelativeFilePath = Path.GetRelativePath(_environment.WebRootPath, path).Replace('\\', '/'),
                        CreatedAt = DateTime.Now
                    });
                }
                await context.JobOfferDocuments.AddRangeAsync(records);
                await context.SaveChangesAsync();
            }
            catch
            {
                // Clean up only files created by this attempt; keep the draft available for retry.
                foreach (var path in savedPaths)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Unable to clean up attachment {Path} after a failed save.", path);
                    }
                }
                throw;
            }
        }

        private async Task<decimal> GetMonthlyRiceAllowance(int jobofferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var compenItem = await context.JOCompanyCompensationItems
                .FirstOrDefaultAsync(jo => jo.JobOfferId == jobofferId && jo.ItemId == 8);

            return compenItem.MonthlyAmount.GetValueOrDefault();
        }

        private async Task<decimal> GetDailyTranspoAllowance(int jobofferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var compenItem = await context.JOCompanyCompensationItems
                .FirstOrDefaultAsync(jo => jo.JobOfferId == jobofferId && jo.ItemId == 9);

            return compenItem.MonthlyAmount.GetValueOrDefault() / 23;
        }

        private static string CreateLetterHtml(string letterBody) => $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="utf-8" />
                <style>
                    @page { size: A4; margin: 15mm; }
                    body { margin: 0; color: #202936; font-family: Arial, Helvetica, sans-serif; font-size: 10pt; line-height: 1.5; }
                    .offer-letter h1 { margin: 0 0 8px; font-size: 21pt; color: #243c56; }
                    .offer-letter h2 { margin: 0 0 16px; font-size: 17pt; }
                    .offer-letter h3 { margin: 22px 0 10px; font-size: 12pt; color: #243c56; }
                    .offer-letter h2, .offer-letter h3 { break-after: avoid; }
                    .offer-letter p { margin: 0 0 12px; orphans: 3; widows: 3; }
                    .offer-letter address { margin: 0; font-style: normal; }
                    .offer-letter table { width: 100%; border-collapse: collapse; margin-bottom: 16px; }
                    .offer-letter th, .offer-letter td { border: 1px solid #dce1e7; padding: 8px 12px; text-align: left; vertical-align: top; overflow-wrap: anywhere; }
                    .offer-letter th { width: 36%; font-weight: 600; }
                    .offer-letter tr, .offer-letter-signatures { break-inside: avoid; }
                    .offer-letter .offer-note { font-size: 10pt; color: #526071; }
                    .offer-letter-signatures { display: flex; gap: 32px; margin-top: 32px; }
                    .offer-letter-signatures > div { flex: 1; padding-top: 10px; border-top: 1px solid #778391; }
                </style>
            </head>
            <body><article class="offer-letter">{{letterBody}}</article></body>
            </html>
            """;

    }
}
