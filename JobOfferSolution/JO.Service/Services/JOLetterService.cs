using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace JO.Service.Services
{
    public class JOLetterService : IJOLetterService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IUtilitiesService _UtilitiesService;

        public JOLetterService(IDbContextFactory<JobOfferDbContext> dbContext, IUtilitiesService UtilitiesService)
        {
            _dbContext = dbContext;
            _UtilitiesService = UtilitiesService;
        }

        public async Task<List<JobOfferDocuments>> GetJobOfferDocuments(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOfferDocuments
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .OrderBy(jo => jo.Id)
                .ToListAsync();
        }

        public async Task<List<JOHasEmailAttach>> GetJOHasEmailAttach(int emailId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOHasEmailAttach
                .AsNoTracking()
                .Where(jo => jo.JOEmailId == emailId)
                .OrderBy(jo => jo.Id)
                .ToListAsync();
        }

        public async Task<JOHasEmailAttach?> GetJOHasEmailAttachById(int attachmentId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOHasEmailAttach
                .AsNoTracking()
                .FirstOrDefaultAsync(attachment => attachment.Id == attachmentId);
        }

        public async Task<int> RemoveOptionAttachment(int emailId, int attachmentId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            // Delete only the email association, preserving the reusable PDF.
            return await context.JOHasEmailAttach
                .Where(attachment => attachment.Id == attachmentId && attachment.JOEmailId == emailId
                    && attachment.RelativePath != null
                    && context.JobOfferHasEmail.Any(email => email.Id == emailId
                        && email.JobOfferId == attachment.JobOfferId && email.StatusId != 2)
                    && context.JobOfferDocuments.Any(document => document.JobOfferId == attachment.JobOfferId
                        && (document.DocumentType == 1 || document.DocumentType == 2) && document.RelativeFilePath != null
                        && document.RelativeFilePath.Replace("\\", "/") == attachment.RelativePath.Replace("\\", "/")))
                .ExecuteDeleteAsync();
        }

        public async Task<int> AddRangeJOHasEmailAttach(List<JOHasEmailAttach> emailAttach)
        {
            ArgumentNullException.ThrowIfNull(emailAttach);
            if (emailAttach.Count == 0)
                return 0;

            await using var context = await _dbContext.CreateDbContextAsync();
            await context.JOHasEmailAttach.AddRangeAsync(emailAttach);
            return await context.SaveChangesAsync();
        }

        public async Task<JobOfferHasEmail> GetJobOfferHasEmail(int emailId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOfferHasEmail.FindAsync(emailId);
        }

        public async Task<JobOfferHasEmail> GetJobOfferHasEmailViaJobOfferId(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOfferHasEmail.FirstOrDefaultAsync(jo => jo.JobOfferId == jobOfferId);
        }

        public async Task AproveJobOfferHasEmail(int emailId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var updated = await context.JobOfferHasEmail
                .Where(email => email.Id == emailId && email.StatusId == 2)
                .ExecuteUpdateAsync(update => update
                    .SetProperty(email => email.StatusId, 3)
                    .SetProperty(email => email.ModifiedAt, DateTime.Now));
            if (updated == 0)
                throw new InvalidOperationException("The email was not found or is no longer awaiting approval.");
        }

        public async Task<List<JOHasEmailStatus>> GetJOHasEmailStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOHasEmailStatus
                .AsNoTracking()
                .OrderBy(jo=>jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<JobOffers>> GetJobOffersForDiscussion()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers
                .AsNoTracking()
                .Where(jo => jo.WorkFlowId == 8) // For Discussion
                .OrderBy(jo => jo.RefNum)
                .ThenBy(jo => jo.Id)
                .ToListAsync();
        }

        public async Task<List<VwJobOfferHasEmail>> GetVwJobOfferHasEmail()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferHasEmail.AsNoTracking().ToListAsync();
        }

        public async Task<List<VwJobOfferHasEmail>> GetApproverJobOfferHasEmail()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            int?[] statusIds = { 2, 3 };
            return await context.VwJobOfferHasEmail
                .AsNoTracking()
                .Where(jo=> statusIds.Contains(jo.StatusId))
                .ToListAsync();
        }

        public async Task<int> UpdateJobOfferHasEmail(JobOfferHasEmail jobOfferEmail)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            jobOfferEmail.ModifiedAt = DateTime.Now;

            context.JobOfferHasEmail.Update(jobOfferEmail);
            return await context.SaveChangesAsync();
        }

        public async Task<int> SaveDraftJobOfferHasEmail(JobOfferHasEmail jobOfferEmail)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            jobOfferEmail.StatusId = 1; // Draft
            jobOfferEmail.CreatedAt = DateTime.Now;

            await context.JobOfferHasEmail.AddAsync(jobOfferEmail);
            await context.SaveChangesAsync();

            return jobOfferEmail.Id;
        }

        public async Task<JobOffers> GetJobOffer(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(jobOfferId);
        }

        public async Task<CandidateEmailTemplate> EditedEmailTemplate(int jobOfferId, int templateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var jobOffer = await context.JobOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(offer => offer.Id == jobOfferId);
            if (jobOffer?.CandidateId is not int candidateId)
                return new CandidateEmailTemplate();

            var candidate = await context.VwDboxCandidates
                .AsNoTracking()
                .FirstOrDefaultAsync(candidate => candidate.Id == candidateId);
            if (candidate is null)
                return new CandidateEmailTemplate();

            var template = await context.CandidateEmailTemplate
                .AsNoTracking()
                .FirstOrDefaultAsync(template => template.Id == templateId);

            if (template is null)
                return new CandidateEmailTemplate();

            template.EmailSubject = ReplaceCandidateEmailTokens(template.EmailSubject, jobOffer, candidate, isHtml: false);
            template.EmailMessage = ReplaceCandidateEmailTokens(template.EmailMessage, jobOffer, candidate, isHtml: true);
            return template;
        }

        private static string ReplaceCandidateEmailTokens(string? message, JobOffers jobOffer,
            VwDboxCandidates candidate, bool isHtml)
        {
            var replacements = new Dictionary<string, string?>
            {
                ["#JORefNum"] = jobOffer.RefNum,
                ["#CandidateName"] = candidate.CandidateName,
                ["#Position"] = candidate.JobPosition,
                ["#SalaryGrade"] = candidate.GradeName,
                ["#Company"] = candidate.Company,
                ["#Department"] = candidate.Department,
                ["#Division"] = candidate.Division
            };

            // Replace once so token-like candidate values remain literal, while preserving template HTML.
            return Regex.Replace(message ?? string.Empty,
                @"#(?:JORefNum|CandidateName|Position|SalaryGrade|Company|Department|Division)\b",
                match =>
                {
                    var value = replacements[match.Value] ?? string.Empty;
                    return isHtml ? WebUtility.HtmlEncode(value) : value;
                });
        }

        public async Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJODboxCandidates.FirstOrDefaultAsync(jo=> jo.Id==jobOfferId);
        }

        public async Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.FirstOrDefaultAsync(jo => jo.Id == candidateId);
        }

        public async Task<CompanyCompensation> GetCompanyCompensation(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompanyCompensation.FindAsync(compensationId);
        }

        public async Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOCompanyCompensation
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId && jo.OptionNumber > 0)
                .OrderBy(jo => jo.OptionNumber)
                .ThenBy(jo => jo.Id)
                .ToListAsync();
        }

        public async Task<List<VwCompanyCompensationItems>> GetVwCompanyCompensationItems(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompanyCompensationItems
                .AsNoTracking()
                .Where(jo => jo.CmpnyCmpnstnId == compensationId)
                .ToListAsync();
        }

        public async Task<List<JOItemLetter>> GetJOItemLetter(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            return await context.JOItemLetter
                .AsNoTracking()
                .Where(letter =>
                    letter.ItemId == 0 ||
                    context.CompanyCompensationItems.Any(item =>
                        item.CmpnyCmpnstnId == compensationId &&
                        item.ItemId == letter.ItemId))
                .OrderBy(letter => letter.DisplayOrder)
                .ToListAsync();
        }

        public void UpdateItemLetterPlaceHolder(List<JOItemLetter> joItemLetter,
            VwDboxCandidates candidate,
            decimal proposedSalary)
        {
            string company = candidate.Company ?? string.Empty;
            string position = candidate.JobPosition ?? string.Empty;
            string division = candidate.Division ?? string.Empty;
            string basicPay = _UtilitiesService.ToPeso(proposedSalary);

            (string PlaceHolder, string Value)[] replacements =
            {
                ("#COMPANY", company),
                ("#POSITION", position),
                ("#DIVISION", division),
                ("#BASICPAY", basicPay)
            };

            ReplaceItemLetterPlaceHolders(joItemLetter, replacements);
        }

        public void UpdateItemLetterPlaceHolder(List<JOItemLetter> joItemLetter,
            VwDboxCandidates candidate,
            decimal proposedSalary,
            decimal monthlyRiceAllowanace,
            decimal dailyTranspoAllowance)
        {
            UpdateItemLetterPlaceHolder(joItemLetter, candidate, proposedSalary);

            (string PlaceHolder, string Value)[] replacements =
            {
                ("#RICEMONTHLY", _UtilitiesService.ToPeso(monthlyRiceAllowanace)),
                ("#TRANSPODAILY", _UtilitiesService.ToPeso(dailyTranspoAllowance))
            };

            ReplaceItemLetterPlaceHolders(joItemLetter, replacements);
        }

        private static void ReplaceItemLetterPlaceHolders(List<JOItemLetter> joItemLetter,
            (string PlaceHolder, string Value)[] replacements)
        {
            foreach (var itemLetter in joItemLetter)
            {
                if (string.IsNullOrEmpty(itemLetter.MessageBody))
                {
                    continue;
                }

                foreach (var replacement in replacements)
                {
                    itemLetter.MessageBody = itemLetter.MessageBody.Replace(
                        replacement.PlaceHolder,
                        replacement.Value,
                        StringComparison.OrdinalIgnoreCase);
                }
            }
        }
    }
}
