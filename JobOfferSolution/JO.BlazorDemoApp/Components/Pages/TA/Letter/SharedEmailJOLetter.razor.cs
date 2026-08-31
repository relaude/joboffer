using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.Letter
{
    public partial class SharedEmailJOLetter
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter, EditorRequired] public VwDboxCandidates Candidate { get; set; } = new();
        [Parameter, EditorRequired] public IReadOnlyList<JOCompanyCompensation> CompensationOptions { get; set; } = [];
        [Parameter] public decimal? InitialSelectedProposedSalary { get; set; }
        [Parameter] public string LetterBody { get; set; } = string.Empty;
        [Parameter] public string InitialEmailSubject { get; set; } = string.Empty;
        [Parameter] public string InitialTestEmailRecipient { get; set; } = string.Empty;
        [Parameter] public EventCallback<decimal> OnCompensationOptionChanged { get; set; }
        [Parameter] public EventCallback<EmailRequest> OnSendTestEmail { get; set; }
        [Parameter] public EventCallback<EmailRequest> OnSendEmail { get; set; }
        [Parameter] public bool IsSending { get; set; }
        [Parameter] public long MaxAttachmentSize { get; set; } = 10 * 1024 * 1024;
        [Parameter] public string? AcceptedFileTypes { get; set; }
        [Parameter] public string ComponentId { get; set; } = "shared-email-jo-letter";

        private TextEditor? letterEditor;
        private string emailSubject = string.Empty;
        private string testEmailRecipient = string.Empty;
        private string loadedLetterBody = string.Empty;
        private decimal selectedProposedSalary;
        private bool loadLetterBody;
        private bool initialized;
        private bool isReadingAttachment;
        private FileStreamDto? attachment;
        private string attachmentContentType = string.Empty;
        private string attachmentError = string.Empty;

        private bool HasLetterContent => !string.IsNullOrWhiteSpace(LetterBody);
        private string CompensationOptionId => $"{ComponentId}-compensation-option";
        private string EmailRecipientId => $"{ComponentId}-email-recipient";
        private string EmailSubjectId => $"{ComponentId}-email-subject";
        private string TestEmailRecipientId => $"{ComponentId}-test-email-recipient";
        private string LetterEditorId => $"{ComponentId}-editor";
        private string AttachmentId => $"{ComponentId}-attachment";

        protected override void OnParametersSet()
        {
            if (!initialized)
            {
                initialized = true;
                emailSubject = InitialEmailSubject;
                testEmailRecipient = InitialTestEmailRecipient;
            }

            var defaultProposedSalary = CompensationOptions
                .Where(item => item.OptionNumber > 0 && item.Declined == false)
                .OrderBy(item => item.OptionNumber)
                .FirstOrDefault()?.ProposedSalary.GetValueOrDefault() ?? 0m;
            selectedProposedSalary = InitialSelectedProposedSalary ?? defaultProposedSalary;

            if (!string.Equals(loadedLetterBody, LetterBody, StringComparison.Ordinal))
            {
                loadedLetterBody = LetterBody;
                loadLetterBody = HasLetterContent;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!loadLetterBody || letterEditor is null)
                return;

            loadLetterBody = false;

            await Task.Delay(500);
            await letterEditor.LoadHTMLContent(LetterBody);
        }

        private async Task ChangeCompensationOptionAsync(decimal proposedSalary)
        {
            selectedProposedSalary = proposedSalary;
            await OnCompensationOptionChanged.InvokeAsync(proposedSalary);
        }

        private async Task SendTestEmailAsync()
        {
            var request = await CreateEmailRequestAsync(testEmailRecipient);
            await OnSendTestEmail.InvokeAsync(request);
        }

        private async Task SendEmailAsync()
        {
            var request = await CreateEmailRequestAsync(Candidate.EmailAddress ?? string.Empty);
            await OnSendEmail.InvokeAsync(request);
        }

        private async Task<EmailRequest> CreateEmailRequestAsync(string recipient)
        {
            var body = letterEditor is null ? LetterBody : await letterEditor.GetHTML();
            return new EmailRequest
            {
                To = recipient.Trim(),
                Subject = emailSubject.Trim(),
                Body = body,
                FileStreams = attachment is null ? null : [attachment]
            };
        }

        private async Task SelectAttachmentAsync(InputFileChangeEventArgs args)
        {
            attachment = null;
            attachmentContentType = string.Empty;
            attachmentError = string.Empty;

            var file = args.File;
            if (file.Size > MaxAttachmentSize)
            {
                attachmentError = $"{file.Name} exceeds the {FormatFileSize(MaxAttachmentSize)} file-size limit.";
                return;
            }

            try
            {
                isReadingAttachment = true;
                await using var source = file.OpenReadStream(MaxAttachmentSize);
                await using var destination = new MemoryStream();
                await source.CopyToAsync(destination);

                attachment = new FileStreamDto
                {
                    Name = file.Name,
                    SizeInKb = FormatFileSize(file.Size),
                    Content = destination.ToArray()
                };
                attachmentContentType = string.IsNullOrWhiteSpace(file.ContentType)
                    ? "Unknown file type"
                    : file.ContentType;
            }
            catch (IOException)
            {
                attachmentError = "The selected file could not be read. Please choose it again.";
            }
            finally
            {
                isReadingAttachment = false;
            }
        }

        private void RemoveAttachment()
        {
            attachment = null;
            attachmentContentType = string.Empty;
            attachmentError = string.Empty;
        }

        private static string FormatFileSize(long bytes)
        {
            const double bytesPerKilobyte = 1024d;
            const double bytesPerMegabyte = bytesPerKilobyte * 1024d;

            return bytes >= bytesPerMegabyte
                ? $"{bytes / bytesPerMegabyte:N2} MB"
                : $"{bytes / bytesPerKilobyte:N2} KB";
        }
    }
}
