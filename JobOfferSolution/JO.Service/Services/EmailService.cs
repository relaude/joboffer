using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Persistence;
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
        private readonly IAppSettings _appSettings;

        public EmailService(IDbContextFactory<JobOfferDbContext> dbContext, IAppSettings appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings;
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

        private string ComposeDivisionHeadL1ApprovalLink(JobOffers jobOffer)
        {
            int workFlowId = 0;

            if (jobOffer.IsHROD == true)
            {
                workFlowId = 15; // For HROD Head 2nd Approval
            }

            if (jobOffer.IsHROD == false)
            {
                workFlowId = jobOffer.OfferRangeId == 1 ? 8 : 6; // For Discussion : For HROD Head Approval
            }

            return ComposeApproveViaEmailLink(jobOffer.Id, workFlowId, roleId: 5, actionId: 4);
        }

        private string ComposeApproveViaEmailLink(int jobOfferId, int workFlowId, int roleId, int actionId)
        {
            var baseUrl = _appSettings.GetBaseUrl().TrimEnd('/');
            var url = $"{baseUrl}/api/Approval/ApproveViaEmail"
                + $"?jobOfferId={jobOfferId}&workFlowId={workFlowId}&roleId={roleId}&actionId={actionId}";

            return $"<a href='{WebUtility.HtmlEncode(url)}'>Approve</a>";
        }

        private string ComposeSendBackViaEmailLink(int jobOfferId, int roleId)
        {
            var baseUrl = _appSettings.GetBaseUrl().TrimEnd('/');
            var url = $"{baseUrl}/api/Approval/SendbackViaEmail"
                + $"?jobOfferId={jobOfferId}&roleId={roleId}";

            return $"<a href='{WebUtility.HtmlEncode(url)}'>Send Back</a>";
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
                ["#CandidateRemarks"] = joAnalysis?.CandidateRemarks
            };

            string? approvalLink = null;
            string? sendBackLink = null;
            if (workFlowId == 5 && Regex.IsMatch(emailTemplate.EmailMessage ?? string.Empty, @"#(?:ApprovalLink|SendBack)\b"))
            {
                approvalLink = ComposeDivisionHeadL1ApprovalLink(jobOffer);
                sendBackLink = ComposeSendBackViaEmailLink(jobOffer.Id, roleId: 5);
            }

            emailTemplate.EmailSubject = ReplaceTemplateTokens(emailTemplate.EmailSubject, replacements, false);
            emailTemplate.EmailMessage = ReplaceTemplateTokens(
                emailTemplate.EmailMessage, replacements, true, ComposeHtmlTable(compensations), approvalLink, sendBackLink);
            emailTemplate.EmailMessage = MinifyEmailMessage(emailTemplate.EmailMessage);

            return emailTemplate;
        }

        private static string MinifyEmailMessage(string? message)
        {
            if (string.IsNullOrEmpty(message))
                return string.Empty;

            // Explicit CSS whitespace rules can make spacing significant throughout the email.
            if (message.Contains("white-space", StringComparison.OrdinalIgnoreCase))
                return message;

            try
            {
                // Keep tags/attributes, Outlook comments, and whitespace-sensitive blocks intact.
                // Only collapse ordinary HTML text whitespace; keep one space between inline elements.
                return Regex.Replace(message,
                    """(?<preserve><!--[\s\S]*?-->|<(?<tag>pre|textarea|script|style)\b[^>]*(?:"[^"]*"|'[^']*')*[^>]*>[\s\S]*?</\k<tag>\s*>|<[^>"']*(?:"[^"]*"[^>"']*|'[^']*'[^>"']*)*>)|(?<text>[^<]+)""",
                    match => match.Groups["preserve"].Success
                        ? match.Value
                        : Regex.Replace(match.Value, @"[ \t\r\n\f]+", " "),
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
                    TimeSpan.FromSeconds(1));
            }
            catch (RegexMatchTimeoutException)
            {
                // Minification is optional; a complex template should still be sent unchanged.
                return message;
            }
        }

        private static string ReplaceTemplateTokens(
            string? template, IReadOnlyDictionary<string, string?> replacements, bool isHtml,
            string htmlTable = "", string? approvalLink = null, string? sendBackLink = null)
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
                @"#(?:CandidateName|Position|SalaryGrade|Department|Division|CandidateRemarks|HtmlTableOffers|ApprovalLink|SendBack)\b",
                match =>
                {
                    if (match.Value == "#HtmlTableOffers")
                        return isHtml ? htmlTable : string.Empty;

                    if (match.Value == "#ApprovalLink")
                        return isHtml && approvalLink != null ? approvalLink : match.Value;

                    if (match.Value == "#SendBack")
                        return isHtml && sendBackLink != null ? sendBackLink : match.Value;

                    var value = replacements.TryGetValue(match.Value, out var replacement)
                        ? replacement ?? string.Empty
                        : string.Empty;
                    return isHtml ? WebUtility.HtmlEncode(value) : value;
                });
        }

        private string ComposeHtmlTable(List<JOCompanyCompensation> joCompanyCompensations)
        {
            if (joCompanyCompensations.Count == 0)
                return string.Empty;

            var options = joCompanyCompensations.OrderBy(item => item.OptionNumber).ToList();
            const string cellStyle = "border:1px solid #d1d5db;padding:8px;vertical-align:top;overflow-wrap:anywhere;";
            var htmlTable = new StringBuilder();
            htmlTable.Append("<table border='1' cellpadding='8' cellspacing='0' width='100%' style='width:100%;border-collapse:collapse;table-layout:fixed;'>");
            htmlTable.Append($"<thead><tr><th scope='col' style='{cellStyle}text-align:left;'>Details</th>");

            foreach (var item in options)
            {
                htmlTable.Append($"<th scope='col' style='{cellStyle}text-align:center;'>Proposed Option {item.OptionNumber}</th>");
            }

            htmlTable.Append("</tr></thead><tbody>");
            AppendRow("Amount", item => item.ProposedSalary?.ToString("N2"));
            AppendRow("% Increase Monthly", item => $"{item.DiffTotalMonthly}%");
            AppendRow("% Increase Annual", item => $"{item.DiffTotalAnnually}%");
            AppendRow("Overtaken Incumbent", item => $"{item.Incumbents}");
            AppendRow("Remarks", item => item.Remarks, "left");
            htmlTable.Append("</tbody></table>");

            return htmlTable.ToString();

            void AppendRow(string label, Func<JOCompanyCompensation, string?> getValue, string alignment = "right")
            {
                htmlTable.Append($"<tr><th scope='row' style='{cellStyle}text-align:left;'>{WebUtility.HtmlEncode(label)}</th>");
                foreach (var item in options)
                {
                    htmlTable.Append($"<td style='{cellStyle}text-align:{alignment};'>{WebUtility.HtmlEncode(getValue(item))}</td>");
                }
                htmlTable.Append("</tr>");
            }
        }
    }
}
