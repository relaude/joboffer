using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace JO.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public EmailService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task TestMailAsync(string recipients)
        {
            string[] arrayRecipients = recipients.Split(';', StringSplitOptions.RemoveEmptyEntries);

            using (SmtpClient client = new SmtpClient(SmtpConstatnts.HostName, SmtpConstatnts.Port))
            {
                client.Credentials = new NetworkCredential(
                    SmtpConstatnts.HostName,
                    SmtpConstatnts.Password
                );

                client.EnableSsl = false;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(SmtpConstatnts.FromMail);

                    foreach (string recipient in arrayRecipients)
                    {
                        mail.To.Add(new MailAddress(recipient.Trim()));
                    }

                    mail.Subject = "Test Email";
                    mail.Body = "Hello from hMailServer";
                    mail.IsBodyHtml = true;

                    await client.SendMailAsync(mail);
                }
            }
        }

        public async Task SendAsync(EmailRequest request)
        {
            using (var client = new SmtpClient(SmtpConstatnts.HostName, SmtpConstatnts.Port))
            {
                client.Credentials = new NetworkCredential(SmtpConstatnts.HostName, SmtpConstatnts.Password);
                client.EnableSsl = false;

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(SmtpConstatnts.FromMail, SmtpConstatnts.DisplayName);

                    AddEmails(mail.To, request.To);
                    AddEmails(mail.CC, request.Cc);
                    AddEmails(mail.Bcc, request.Bcc);

                    mail.Subject = request.Subject;
                    mail.Body = request.Body;
                    mail.IsBodyHtml = true;

                    // Attach FileStreamDto files
                    if (request.FileStreams != null)
                    {
                        foreach (var file in request.FileStreams)
                        {
                            if (file.Content != null && file.Content.Length > 0)
                            {
                                var stream = new MemoryStream(file.Content);
                                mail.Attachments.Add(new Attachment(stream, file.Name));
                            }
                        }
                    }

                    await client.SendMailAsync(mail);
                }
            }
        }

        public async Task<int> SendAsync(string recipients,
            string subject,
            string body)
        {
            var parameters = new
            {
                Profile_name = "HRSMTP",
                Recipients = recipients,
                Body = body,
                Body_format = "HTML",
                Subject = subject,
                From_address = "",
                Blind_copy_recipients = "",
                Importance = "Normal",
                Reply_to = ""
            };

            //exec msdb.dbo.sp_send_dbmail
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Database.ExecuteSqlInterpolatedAsync($"""
                EXEC msdb.dbo.sp_send_dbmail
                    @profile_name = {parameters.Profile_name},
                    @recipients = {parameters.Recipients},
                    @body = {parameters.Body},
                    @body_format = {parameters.Body_format},
                    @subject = {parameters.Subject},
                    @from_address = {parameters.From_address},
                    @blind_copy_recipients = {parameters.Blind_copy_recipients},
                    @importance = {parameters.Importance},
                    @reply_to = {parameters.Reply_to}
                """);
        }

        public async Task SendJOEmailNotification(int jobOfferId, int workFlowId)
        {
            EmailTemplate template = await EditedEmailTemplate(jobOfferId, workFlowId);

            if (string.IsNullOrEmpty(template.EmailSubject)) return;
            
            EmailRequest request = new();
            request.To = template.OtherRecipient;
            request.Subject = template.EmailSubject;
            request.Body = template.EmailMessage;

            try
            {
                await SendAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void AddEmails(MailAddressCollection collection, string? emails)
        {
            if (string.IsNullOrWhiteSpace(emails))
                return;

            var list = emails.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var email in list)
            {
                collection.Add(new MailAddress(email.Trim()));
            }
        }

        private async Task<EmailTemplate> EditedEmailTemplate(int jobOfferId, int workFlowId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var emailTemplate = await context.EmailTemplate
                .AsNoTracking()
                .FirstOrDefaultAsync(jo => jo.WorkFlowId == workFlowId
                    && jo.IsActive == true);

            if (emailTemplate == null) return new EmailTemplate();

            var jobOffer = await context.JobOffers.FindAsync(jobOfferId);
            if (jobOffer?.CandidateId is not int candidateId) return new EmailTemplate();

            var joAnalysis = await context.JOAnalysis.FirstOrDefaultAsync(jo => jo.JobOfferId == jobOfferId);

            var candidate = await context.VwDboxCandidates
                .FirstOrDefaultAsync(jo => jo.Id == candidateId);

            if (candidate is null) return new EmailTemplate();

            var compensations = await context.JOCompanyCompensation
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId && jo.OptionNumber > 0)
                .OrderBy(jo => jo.OptionNumber)
                .ToListAsync();

            var replacements = new Dictionary<string, string?>
            {
                ["#CandidateName"] = candidate.CandidateName,
                ["#Position"] = candidate.JobPosition,
                ["#SalaryGrade"] = candidate.GradeName,
                ["#Department"] = candidate.Department,
                ["#Division"] = candidate.Division,
                ["#CandidateRemarks"] = joAnalysis?.CandidateReamrks
            };

            emailTemplate.EmailSubject = ReplaceTemplateTokens(emailTemplate.EmailSubject, replacements, false);
            emailTemplate.EmailMessage = ReplaceTemplateTokens(
                emailTemplate.EmailMessage, replacements, true, ComposeHtmlTable(compensations));

            return emailTemplate;
        }

        private static string ReplaceTemplateTokens(
            string? template, IReadOnlyDictionary<string, string?> replacements, bool isHtml, string htmlTable = "")
        {
            var content = template ?? string.Empty;
            if (isHtml)
            {
                // A table cannot be nested in the paragraph produced by the rich-text editor.
                content = Regex.Replace(content, @"<p\b[^>]*>\s*#HtmlTableOffers\s*</p>",
                    "#HtmlTableOffers", RegexOptions.IgnoreCase);
            }

            // Replace in one pass so placeholder-like text in candidate data stays literal.
            return Regex.Replace(content,
                @"#(?:CandidateName|Position|SalaryGrade|Department|Division|CandidateRemarks|HtmlTableOffers)\b",
                match =>
                {
                    if (match.Value == "#HtmlTableOffers")
                        return isHtml ? htmlTable : string.Empty;

                    var value = replacements.TryGetValue(match.Value, out var replacement)
                        ? replacement ?? string.Empty
                        : string.Empty;
                    return isHtml ? WebUtility.HtmlEncode(value) : value;
                });
        }

        private string ComposeHtmlTable(List<JOCompanyCompensation> joCompanyCompensations)
        {
            string htmlTable = "";

            foreach (var item in joCompanyCompensations)
            {
                htmlTable = htmlTable + $@"<table border='1'>
                  <thead>
                    <tr>
                      <th colspan='2'>Option {item.OptionNumber}</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td>Amount</td>
                      <td>{item.ProposedSalary?.ToString("N2")}</td>
                    </tr>
                    <tr>
                      <td>% Increase Monthly</td>
                      <td>{item.DiffTotalMonthly}%</td>
                    </tr>
                    <tr>
                      <td>% Increase Annual</td>
                      <td>{item.DiffTotalAnnually}%</td>
                    </tr>
                    <tr>
                      <td>Overtaken Incumbent</td>
                      <td>{item.Incumbents}</td>
                    </tr>
                    <tr>
                      <td colspan='2'>Remarks</td>
                    </tr>
                    <tr>
                      <td colspan='2'>{WebUtility.HtmlEncode(item.Remarks)}</td>
                    </tr>
                  </tbody>
                </table><br />";
            }

            return htmlTable;
        }
    }
}
