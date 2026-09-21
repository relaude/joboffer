using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class TAPreviewJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ILogger<TAPreviewJOEmail> Logger { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        [Parameter] public int emailId { get; set; }

        private JobOfferHasEmail jobOfferEmail = new();
        private List<JOHasEmailAttach> attachments = new();
        private bool isLoading = true;
        private bool isDownloadingAttachment;
        private string? loadError;
        private string? attachmentError;

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
                if (email is null)
                {
                    loadError = "This email could not be found.";
                    return;
                }
                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(emailId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email {EmailId} for preview.", emailId);
                loadError = "Unable to load this email. Please reload the page and try again.";
            }
            finally
            {
                isLoading = false;
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
