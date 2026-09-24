using JO.DataModel.Entity;
using JO.DataModel.DTOs;
using JO.Service.Enum;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;
using Microsoft.JSInterop;

namespace JO.BlazorDemoApp.Components.Pages.Letter
{
    public partial class CandidateJOLetter
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private ILogger<CandidateJOLetter> Logger { get; set; } = default!;

        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;

        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }
        [Parameter] public int RoleId { get; set; }

        private readonly string componentId = $"candidate-jo-letter-{Guid.NewGuid():N}";
        private string ElementId(string suffix) => $"{componentId}-{suffix}";
        private int? loadedJobOfferId;
        private JobOfferHasEmail jobOfferEmail = new();
        private TextEditor? messageEditor;
        private bool isLoadingMessage = true;
        private bool loadMessageContent;
        private bool isMessageEditorReady;
        private string? loadError;
        private int userId;
        private bool isSaving;
        private bool isSending;
        private bool isSent;
        private bool IsReadOnly => jobOfferEmail.StatusId == (int)EnumJOEmailStatus.ForApproval
            || jobOfferEmail.StatusId == (int)EnumJOEmailStatus.Approved;
        private bool CanSend => !isLoadingMessage && !isSending && !isSent && loadError is null
            && jobOfferEmail.StatusId == (int)EnumJOEmailStatus.Approved;
        private bool isSendingTestMail;
        private string testEmailRecipient = string.Empty;
        private bool isValidating;
        private List<JOHasEmailAttach> attachments = new();
        private bool isDownloadingAttachment;
        private string? attachmentError;
        private Shared.JOModal? attachOptionModal;
        private List<JobOfferDocuments> optionDocuments = new();
        private List<JobOfferDocuments> jobOfferDocuments = new();
        private int? removingAttachmentId;
        private int? selectedOptionDocumentId;
        private bool isLoadingOptions;
        private bool isAttachingOption;
        private string? optionsLoadError;
        private string? attachOptionError;

        private bool IsAttachOptionDisabled => IsActionDisabled || isLoadingOptions
            || IsReadOnly || jobOfferEmail.JobOfferId is null;

        private bool IsOptionAttached(JobOfferDocuments document) => attachments.Any(attachment =>
            string.Equals(attachment.RelativePath?.Replace('\\', '/'),
                document.RelativeFilePath?.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase));

        private bool IsOptionAttachment(JOHasEmailAttach attachment)
        {
            if (string.IsNullOrWhiteSpace(attachment.RelativePath))
                return false;

            var documents = jobOfferDocuments.Where(document =>
                string.Equals(document.RelativeFilePath?.Replace('\\', '/'),
                    attachment.RelativePath.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase));
            return documents.Any(document => document.DocumentType == 1)
                && !documents.Any(document => document.DocumentType == 2);
        }

        private async Task RemoveOptionAttachmentAsync(JOHasEmailAttach attachment)
        {
            if (IsAttachOptionDisabled || !attachments.Contains(attachment)
                || !(IsOptionAttachment(attachment) || IsPdfAttachmentOfType(attachment, 2)))
                return;

            var email = jobOfferEmail;
            removingAttachmentId = attachment.Id;
            attachmentError = null;
            try
            {
                var removed = await JOLetterService.RemoveOptionAttachment(email.Id, attachment.Id);
                if (!ReferenceEquals(email, jobOfferEmail))
                    return;

                if (removed > 0)
                    attachments.Remove(attachment);
                else
                    attachmentError = "The attachment could not be removed. Reload the page to check its current status.";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to remove attachment {AttachmentId} from email {EmailId}.", attachment.Id, email.Id);
                if (ReferenceEquals(email, jobOfferEmail))
                    attachmentError = "Unable to remove the attachment. Please try again.";
            }
            finally
            {
                removingAttachmentId = null;
            }
        }

        private async Task ShowAttachOptionAsync()
        {
            if (IsAttachOptionDisabled || jobOfferEmail.JobOfferId is not int jobOfferId)
                return;

            var email = jobOfferEmail;
            selectedOptionDocumentId = null;
            optionDocuments.Clear();
            optionsLoadError = null;
            attachOptionError = null;
            isLoadingOptions = true;
            attachOptionModal?.Show();
            try
            {
                var documents = await JOLetterService.GetJobOfferDocuments(jobOfferId);
                if (ReferenceEquals(email, jobOfferEmail))
                {
                    jobOfferDocuments = documents;
                    optionDocuments = documents.Where(document => (document.DocumentType == 1 || document.DocumentType == 2)
                        && !string.IsNullOrWhiteSpace(document.RelativeFilePath)).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load option PDFs for job offer {JobOfferId}.", jobOfferId);
                if (ReferenceEquals(email, jobOfferEmail))
                    optionsLoadError = "Unable to load PDFs. Please try again.";
            }
            finally
            {
                isLoadingOptions = false;
            }
        }

        private async Task AttachOptionAsync()
        {
            if (IsAttachOptionDisabled || selectedOptionDocumentId is not int documentId)
                return;

            var document = optionDocuments.FirstOrDefault(item => item.Id == documentId);
            if (document is null || IsOptionAttached(document))
                return;

            var email = jobOfferEmail;
            isAttachingOption = true;
            attachOptionError = null;
            try
            {
                // Refresh before inserting so reopening the modal does not attach the same file twice.
                var currentAttachments = await JOLetterService.GetJOHasEmailAttach(email.Id);
                if (!ReferenceEquals(email, jobOfferEmail))
                    return;
                attachments = currentAttachments;
                if (document.DocumentType == 1 && attachments.Any(IsOptionAttachment))
                {
                    attachOptionError = "Only one option PDF is allowed. Remove the existing option before attaching another.";
                    return;
                }
                if (IsOptionAttached(document))
                {
                    attachOptionError = "This PDF is already attached.";
                    return;
                }

                if (document.DocumentType == 2 && attachments.Any(attachment => IsPdfAttachmentOfType(attachment, 2)))
                {
                    attachOptionError = "Only one benefits PDF is allowed. Remove the existing benefits PDF before attaching another.";
                    return;
                }

                var attachment = new JOHasEmailAttach
                {
                    JOEmailId = email.Id,
                    JobOfferId = email.JobOfferId,
                    FileName = document.FileName,
                    RelativePath = document.RelativeFilePath
                };
                // Verify the existing file can be downloaded before persisting its attachment record.
                using (File.OpenRead(GetAttachmentPath(attachment))) { }
                await JOLetterService.AddRangeJOHasEmailAttach(new List<JOHasEmailAttach> { attachment });
                if (ReferenceEquals(email, jobOfferEmail))
                {
                    attachments.Add(attachment);
                    attachOptionModal?.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to attach option document {DocumentId} to email {EmailId}.", documentId, email.Id);
                if (ReferenceEquals(email, jobOfferEmail))
                    attachOptionError = "Unable to attach the PDF. Check that the file is available and try again.";
            }
            finally
            {
                isAttachingOption = false;
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

        private static bool IsPdfAttachment(JOHasEmailAttach attachment) =>
            string.Equals(Path.GetExtension(attachment.RelativePath), ".pdf", StringComparison.OrdinalIgnoreCase);

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


        private bool IsActionDisabled => IsReadOnly || isSending || isLoadingMessage || !isMessageEditorReady || isValidating || isSaving || isSendingTestMail || isAttachingOption || removingAttachmentId.HasValue || loadError is not null;

        protected override async Task OnParametersSetAsync()
        {
            if (loadedJobOfferId == jobOfferId)
                return;
            loadedJobOfferId = jobOfferId;
            isSent = false;
            isLoadingMessage = true;
            loadMessageContent = false;
            isMessageEditorReady = false;
            messageEditor = null;
            loadError = null;
            jobOfferEmail = new();
            attachments.Clear();
            attachmentError = null;
            attachOptionModal?.Close();
            optionDocuments.Clear();
            jobOfferDocuments.Clear();
            selectedOptionDocumentId = null;
            optionsLoadError = null;
            attachOptionError = null;
            try
            {
                userId = await AccountService.GetJobOfferUserId();
                var email = await JOLetterService.GetJobOfferHasEmailViaJobOfferId(jobOfferId);
                if (email is null)
                {
                    loadError = "The job offer email was not found.";
                    return;
                }
                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(email.Id);
                if (email.JobOfferId is int emailJobOfferId)
                {
                    jobOfferDocuments = await JOLetterService.GetJobOfferDocuments(emailJobOfferId);
                }
                loadMessageContent = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email for job offer {JobOfferId}.", jobOfferId);
                loadError = "Unable to load the job offer email. Please reload the page and try again.";
            }
            finally
            {
                isLoadingMessage = false;
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!loadMessageContent || messageEditor is null)
                return;

            loadMessageContent = false;
            var editor = messageEditor;
            // Match the existing template editor's Quill initialization timing.
            await Task.Delay(500);
            if (ReferenceEquals(editor, messageEditor))
            {
                await editor.LoadHTMLContent(jobOfferEmail.EmailMessage ?? string.Empty);
                if (ReferenceEquals(editor, messageEditor))
                {
                    isMessageEditorReady = true;
                    StateHasChanged();
                }
            }
        }

        private async Task UpdateEmailMessageAsync()
        {
            if (IsReadOnly || messageEditor is null || !isMessageEditorReady)
                return;

            var model = jobOfferEmail;
            var html = await messageEditor.GetHTML();
            if (ReferenceEquals(model, jobOfferEmail))
                model.EmailMessage = html;
        }

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

        private async Task TestMailAsync()
        {
            if (IsActionDisabled || messageEditor is null)
                return;

            isSendingTestMail = true;
            try
            {
                await UpdateEmailMessageAsync();
                var messageText = await messageEditor.GetText();
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(testEmailRecipient))
                    errors.Add("Test Recipient Email Address is required.");
                else if (!UtilitiesService.IsValidEmail(testEmailRecipient.Trim()))
                    errors.Add("Test Recipient Email Address must be a valid email address.");

                if (string.IsNullOrWhiteSpace(jobOfferEmail.Subject))
                    errors.Add("Subject is required.");

                if (string.IsNullOrWhiteSpace(messageText))
                    errors.Add("Message is required.");

                if (errors.Count > 0)
                {
                    await AlertService.Errors(errors, "Validation Errors");
                    return;
                }

                var request = new EmailRequest
                {
                    To = testEmailRecipient.Trim(),
                    Subject = jobOfferEmail.Subject!.Trim(),
                    Body = jobOfferEmail.EmailMessage ?? string.Empty,
                    FileStreams = await LoadAttachmentFilesAsync()
                };

                await EmailService.SendAsync(request);
                await AlertService.Success("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send the job offer test email.");
                await AlertService.Error("The test email could not be sent. Check that all attachment files are available and try again.", "Email Error");
            }
            finally
            {
                isSendingTestMail = false;
            }
        }

        private async Task SendAsync()
        {
            if (!CanSend)
                return;

            var email = jobOfferEmail;
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
                email.StatusId = (int)EnumJOEmailStatus.Draft;
                email.ModifiedBy = userId;
                email.ModifiedAt = DateTime.Now;
                
                try
                {
                    await JOLetterService.UpdateJobOfferHasEmail(email, RoleId, 14, userId, email.Subject);
                }
                catch
                {
                    email.StatusId = (int)EnumJOEmailStatus.Approved;
                    throw;
                }
                if (ReferenceEquals(email, jobOfferEmail))
                {
                    messageEditor = null;
                    isMessageEditorReady = false;
                    loadMessageContent = true;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send job offer email {EmailId}.", email.Id);
                await AlertService.Error(isSent
                    ? "The email was sent, but its status could not be reset to Draft. Do not resend it; reload and check the status."
                    : "The email could not be sent. Check that all attachment files are available and try again.", "Email Error");
                return;
            }
            finally
            {
                isSending = false;
            }

            await AlertService.Success("Job offer email sent successfully. It is now a draft for further updates and approval.", "Sent");
        }

        private Task SaveAsync() => UpdateEmailAsync(1);

        private Task SubmitForApprovalAsync() => UpdateEmailAsync(2);

        private async Task UpdateEmailAsync(int statusId)
        {
            if (IsReadOnly || !await ValidateEmailAsync())
                return;

            isSaving = true;
            var previousStatus = jobOfferEmail.StatusId;
            try
            {
                var submit = statusId == 2;
                if (!await AlertService.Confirm(
                    submit ? "Submit this job offer email for approval?" : "Save changes to this job offer email as a draft?",
                    submit ? "Submit for Approval" : "Save Draft", "Cancel"))
                    return;

                jobOfferEmail.StatusId = statusId;
                jobOfferEmail.ModifiedBy = userId;

                var actionLog = new JOActionLogs
                {
                    JobOfferId = jobOfferId,
                    RoleId = 1, //TA Partner
                    ActionId = 11, //Email Approval Submitted
                    ActionAt = DateTime.Now,
                    ActionBy = userId,
                    Remarks = jobOfferEmail.Subject
                };

                await JOLetterService.UpdateJobOfferHasEmail(jobOfferEmail);
                await JOLetterService.CreateJOActionLogs(actionLog);
                
                if (statusId == (int)EnumJOEmailStatus.ForApproval)
                    isSent = false;
                await AlertService.Success(
                    submit ? "Job offer email submitted for approval." : "Job offer email draft updated successfully.",
                    submit ? "Submitted for Approval" : "Draft Saved");
            }
            catch (Exception ex)
            {
                jobOfferEmail.StatusId = previousStatus;
                Logger.LogError(ex, "Failed to update job offer email {EmailId}.", jobOfferEmail.Id);
                await AlertService.Error("The email could not be updated. Please try again.", "Update Error");
            }
            finally
            {
                isSaving = false;
            }
        }

        private async Task<bool> ValidateEmailAsync()
        {
            if (IsActionDisabled || messageEditor is null)
                return false;

            isValidating = true;
            try
            {
                // Read the latest editor content, including edits immediately before the click.
                await UpdateEmailMessageAsync();
                var messageText = await messageEditor.GetText();
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(jobOfferEmail.ToRecipient))
                    errors.Add("To is required.");
                else if (!IsValidRecipientList(jobOfferEmail.ToRecipient))
                    errors.Add("To must contain valid email addresses separated by semicolons (;).");

                if (!string.IsNullOrWhiteSpace(jobOfferEmail.CCRecipient)
                    && !IsValidRecipientList(jobOfferEmail.CCRecipient))
                    errors.Add("CC must contain valid email addresses separated by semicolons (;).");

                if (string.IsNullOrWhiteSpace(jobOfferEmail.Subject))
                    errors.Add("Subject is required.");

                // Quill's empty HTML (such as <p><br></p>) is not a message.
                if (string.IsNullOrWhiteSpace(messageText))
                    errors.Add("Message is required.");

                var benefitsPdfCount = attachments.Count(attachment =>
                    IsPdfAttachmentOfType(attachment, 2));
                var optionPdfCount = attachments.Count(attachment =>
                    IsPdfAttachmentOfType(attachment, 1));

                if (optionPdfCount != 1 || benefitsPdfCount > 1
                    || attachments.Count != optionPdfCount + benefitsPdfCount)
                    errors.Add("Exactly one option PDF is required. One benefits PDF may also be attached; benefits are optional.");

                if (errors.Count > 0)
                {
                    await AlertService.Errors(errors, "Validation Errors");
                    return false;
                }

                return true;
            }
            finally
            {
                isValidating = false;
            }
        }

        private bool IsPdfAttachmentOfType(JOHasEmailAttach attachment, int documentType)
        {
            if (string.IsNullOrWhiteSpace(attachment.RelativePath)
                || !string.Equals(Path.GetExtension(attachment.FileName), ".pdf", StringComparison.OrdinalIgnoreCase)
                || !string.Equals(Path.GetExtension(attachment.RelativePath), ".pdf", StringComparison.OrdinalIgnoreCase))
                return false;

            var matchingTypes = jobOfferDocuments
                .Where(document => string.Equals(document.RelativeFilePath?.Replace('\\', '/'),
                    attachment.RelativePath.Replace('\\', '/'), StringComparison.OrdinalIgnoreCase))
                .Select(document => document.DocumentType)
                .Distinct()
                .ToList();
            return matchingTypes.Count == 1 && matchingTypes[0] == documentType;
        }

        private bool IsValidRecipientList(string recipients) =>
            recipients.Split(';').Any(email => !string.IsNullOrWhiteSpace(email))
            && UtilitiesService.IsValidSeriesOfEmail(recipients, ';');
    }
}







