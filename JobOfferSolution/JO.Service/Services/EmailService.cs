using JO.DataModel.DTOs;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JO.Service.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {

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
