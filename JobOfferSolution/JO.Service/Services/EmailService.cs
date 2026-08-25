using JO.DataModel.DTOs;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

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
    }
}
