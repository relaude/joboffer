using JO.DataModel.Entity;
using JO.DataModel.DTOs;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class TASendJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private ILogger<TASendJOEmail> Logger { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter] public int emailId { get; set; }

        private JobOfferHasEmail jobOfferEmail = new();
        private List<JOHasEmailAttach> attachments = new();
        private bool isLoading = true;
        private bool isSending;
        private bool isSent;
        private bool isDownloadingAttachment;
        private string? loadError;
        private string? attachmentError;
        private bool CanSend => !isLoading && !isSending && loadError is null && !isSent && jobOfferEmail.StatusId == 3;

        private void GoBack() => Navigation.NavigateTo(JORoutes.TAPartner.JobOfferEmails);

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            isSent = false;
            loadError = null;
            attachmentError = null;
            jobOfferEmail = new();
            attachments.Clear();
            try
            {
                var email = await JOLetterService.GetJobOfferHasEmail(emailId);
                if (email is null || email.StatusId != 3)
                {
                    loadError = "Only approved emails are available to send.";
                    return;
                }
                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(emailId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email {EmailId} for sending.", emailId);
                loadError = "Unable to load this email. Please reload the page and try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SendAsync()
        {
            if (!CanSend)
                return;

            isSending = true;
            try
            {
                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(jobOfferEmail.ToRecipient)
                    || !IsValidRecipientList(jobOfferEmail.ToRecipient))
                    errors.Add("To must contain valid email addresses separated by semicolons (;).");
                if (!string.IsNullOrWhiteSpace(jobOfferEmail.CCRecipient)
                    && !IsValidRecipientList(jobOfferEmail.CCRecipient))
                    errors.Add("CC must contain valid email addresses separated by semicolons (;).");
                if (string.IsNullOrWhiteSpace(jobOfferEmail.Subject))
                    errors.Add("Subject is required.");
                if (string.IsNullOrWhiteSpace(jobOfferEmail.EmailMessage))
                    errors.Add("Message is required.");
                if (errors.Count > 0)
                {
                    await AlertService.Errors(errors, "Validation Errors");
                    return;
                }

                if (!await AlertService.Confirm("Send this job offer email to the listed To and CC recipients?", "Send", "Cancel"))
                    return;

                var request = new EmailRequest
                {
                    To = jobOfferEmail.ToRecipient!.Trim(),
                    Cc = jobOfferEmail.CCRecipient,
                    Subject = jobOfferEmail.Subject!.Trim(),
                    Body = jobOfferEmail.EmailMessage!,
                    FileStreams = await LoadAttachmentFilesAsync()
                };
                await EmailService.SendAsync(request);
                isSent = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send job offer email {EmailId}.", emailId);
                await AlertService.Error("The email could not be sent. Check that all attachment files are available and try again.", "Email Error");
                return;
            }
            finally
            {
                isSending = false;
            }

            await AlertService.Success("Job offer email sent successfully.", "Sent");
        }

        private bool IsValidRecipientList(string recipients) =>
            recipients.Split(';').Any(email => !string.IsNullOrWhiteSpace(email))
            && UtilitiesService.IsValidSeriesOfEmail(recipients, ';');

        private async Task<List<FileStreamDto>> LoadAttachmentFilesAsync()
        {
            var files = new List<FileStreamDto>();
            foreach (var attachment in attachments)
            {
                var content = await File.ReadAllBytesAsync(GetAttachmentPath(attachment));
                files.Add(new FileStreamDto
                {
                    Name = attachment.FileName ?? Path.GetFileName(attachment.RelativePath)!,
                    Content = content,
                    SizeInKb = (content.Length / 1024d).ToString("N0")
                });
            }
            return files;
        }
        private string GetAttachmentPath(JOHasEmailAttach attachment)
        {
            if (string.IsNullOrWhiteSpace(attachment.RelativePath) || Path.IsPathRooted(attachment.RelativePath))
                throw new InvalidOperationException("The attachment path is invalid.");

            var docsRoot = Path.GetFullPath(Path.Combine(Environment.WebRootPath, "docs")) + Path.DirectorySeparatorChar;
            var path = Path.GetFullPath(Path.Combine(Environment.WebRootPath, attachment.RelativePath.Replace('/', Path.DirectorySeparatorChar)));
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (!path.StartsWith(docsRoot, comparison))
                throw new InvalidOperationException("The attachment must be stored in the docs directory.");
            return path;
        }

        private async Task DownloadAttachmentAsync(JOHasEmailAttach attachment)
        {
            if (isDownloadingAttachment)
                return;
            isDownloadingAttachment = true;
            attachmentError = null;
            try
            {
                await using var module = await JS.InvokeAsync<IJSObjectReference>("import",
                    Navigation.ToAbsoluteUri("Components/Pages/TA/JobOfferEmail/TANewJOEmail.razor.js").AbsoluteUri);
                await using var stream = File.OpenRead(GetAttachmentPath(attachment));
                using var reference = new DotNetStreamReference(stream);
                await module.InvokeVoidAsync("downloadAttachment", attachment.FileName ?? Path.GetFileName(stream.Name), reference);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to download email attachment {AttachmentId}.", attachment.Id);
                attachmentError = "Unable to download the attachment. The file may be missing or unavailable.";
            }
            finally
            {
                isDownloadingAttachment = false;
            }
        }
    }
}

