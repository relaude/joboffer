using JO.DataModel.Entity;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JO.BlazorDemoApp.Components.Pages.TALead.JobOfferEmail
{
    public partial class ApproveJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private ILogger<ApproveJOEmail> Logger { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        [Parameter] public int emailId { get; set; }

        private JobOfferHasEmail jobOfferEmail = new();
        private List<JOHasEmailAttach> attachments = new();
        private bool isLoading = true;
        private bool isApproving;
        private bool isDownloadingAttachment;
        private string? loadError;
        private string? attachmentError;
        private bool CanApprove => !isLoading && !isApproving && loadError is null && jobOfferEmail.StatusId == 2;

        private void GoBack() => Navigation.NavigateTo(JORoutes.TALead.JobOfferEmails);

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            loadError = null;
            attachmentError = null;
            jobOfferEmail = new();
            attachments.Clear();
            try
            {
                var email = await JOLetterService.GetJobOfferHasEmail(emailId);
                if (email is null || email.StatusId is not (2 or 3))
                {
                    loadError = "This email is not available for approval.";
                    return;
                }
                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(emailId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email {EmailId} for approval.", emailId);
                loadError = "Unable to load this email. Please reload the page and try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task ApproveAsync()
        {
            if (!CanApprove)
                return;
            isApproving = true;
            var approvingEmailId = emailId;
            try
            {
                if (!await AlertService.Confirm("Approve this job offer email?", "Approve", "Cancel"))
                    return;
                await JOLetterService.AproveJobOfferHasEmail(approvingEmailId);
                jobOfferEmail.StatusId = 3;
                await AlertService.Success("Job offer email approved successfully.", "Approved");
                GoBack();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to approve job offer email {EmailId}.", approvingEmailId);
                await AlertService.Error("The email could not be approved. Please reload the page and try again.", "Approval Error");
            }
            finally
            {
                isApproving = false;
            }
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
