using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;
using Microsoft.JSInterop;
using System.Globalization;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class TANewJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private ILogger<TANewJOEmail> Logger { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private IHtmlToPDFServices HtmlToPDFServices { get; set; } = default!;
        [Inject] private IJOFileService JOFileService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }

        private int templateId = 1;
        private JobOffers? jobOffer;
        private VwDboxCandidates? candidate;
        private int userId;
        private JobOfferHasEmail jobOfferEmail = new();
        private TextEditor? messageEditor;
        private bool isLoadingMessage = true;
        private bool loadMessageContent;
        private bool isMessageEditorReady;
        private bool isValidating;
        private string testEmailRecipient = string.Empty;
        private bool isSendingTestMail;
        private bool isSaving;
        private bool hasSavedDraft;
        private FileStreamDto? benefitsAttachment;
        private bool isLoadingAttachment;
        private string? attachmentError;
        private Shared.JOModal? attachOptionModal;
        private List<JOCompanyCompensation> compensationOptions = new();
        private decimal? selectedProposedSalary;
        private bool isLoadingOptions;
        private string? optionsLoadError;
        private ElementReference optionSelect;
        private bool isAttachingOption;
        private string? attachOptionError;
        private readonly List<FileStreamDto> optionAttachments = new();
        private bool isDownloadingAttachment;
        private string? attachmentDownloadError;

        private void RemoveOptionAttachment(FileStreamDto attachment)
        {
            if (isAttachingOption || isSendingTestMail || isSaving || isValidating)
                return;

            optionAttachments.Remove(attachment);
            attachmentDownloadError = null;
        }

        private async Task DownloadOptionAttachmentAsync(FileStreamDto attachment)
        {
            if (isDownloadingAttachment)
                return;

            isDownloadingAttachment = true;
            attachmentDownloadError = null;
            try
            {
                await using var module = await JS.InvokeAsync<IJSObjectReference>("import",
                    Navigation.ToAbsoluteUri("Components/Pages/TA/JobOfferEmail/TANewJOEmail.razor.js").AbsoluteUri);
                using var stream = new MemoryStream(attachment.Content, writable: false);
                using var streamReference = new DotNetStreamReference(stream);
                await module.InvokeVoidAsync("downloadAttachment", attachment.Name, streamReference);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to download attachment {FileName}.", attachment.Name);
                attachmentDownloadError = "Unable to download the attachment. Please try again.";
            }
            finally
            {
                isDownloadingAttachment = false;
            }
        }
        private bool IsActionDisabled => isLoadingMessage || !isMessageEditorReady || isValidating || isSendingTestMail || isSaving
            || isLoadingAttachment || benefitsAttachment is null || isAttachingOption;

        protected override async Task OnParametersSetAsync()
        {
            isLoadingMessage = true;
            loadMessageContent = false;
            isMessageEditorReady = false;
            messageEditor = null;
            jobOffer = null;
            candidate = null;
            jobOfferEmail = new() { JobOfferId = jobOfferId };
            hasSavedDraft = false;
            attachOptionModal?.Close();
            compensationOptions.Clear();
            selectedProposedSalary = null;
            optionAttachments.Clear();
            attachmentDownloadError = null;

            try
            {
                await LoadBenefitsAttachmentAsync();
                userId = await AccountService.GetJobOfferUserId();
                jobOfferEmail.CreatedBy = userId;

                jobOffer = await JOLetterService.GetJobOffer(jobOfferId);
                if (jobOffer?.CandidateId is not int candidateId)
                    return;

                jobOfferEmail.CandidateId = candidateId;
                candidate = await JOLetterService.GetVwDboxCandidate(candidateId);
                if (candidate is null)
                    return;

                jobOfferEmail.ToRecipient = candidate.EmailAddress;

                var emailTemplate = await JOLetterService.EditedEmailTemplate(jobOfferId, templateId);
                jobOfferEmail.CCRecipient = emailTemplate.CCRecipient;
                jobOfferEmail.Subject = emailTemplate.EmailSubject;
                jobOfferEmail.EmailMessage = emailTemplate.EmailMessage;
            }
            finally
            {
                isLoadingMessage = false;
                loadMessageContent = true;
            }
        }

        private async Task ShowAttachOptionAsync()
        {
            if (isLoadingOptions || isAttachingOption)
                return;

            selectedProposedSalary = null;
            compensationOptions.Clear();
            optionsLoadError = null;
            attachOptionError = null;
            isLoadingOptions = true;
            attachOptionModal?.Show();
            var requestedJobOfferId = jobOfferId;
            try
            {
                var options = await JOLetterService.GetJOCompanyCompensation(requestedJobOfferId);
                if (requestedJobOfferId == jobOfferId)
                    compensationOptions = options.Where(option => option.ProposedSalary.HasValue).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load compensation options for job offer {JobOfferId}.", requestedJobOfferId);
                if (requestedJobOfferId == jobOfferId)
                    optionsLoadError = "Unable to load options. Please try again.";
            }
            finally
            {
                isLoadingOptions = false;
            }
        }

        private async Task AttachOptionAsync()
        {
            if (isAttachingOption || isLoadingOptions || selectedProposedSalary is not decimal salary)
                return;

            isAttachingOption = true;
            attachOptionError = null;
            var requestedJobOfferId = jobOfferId;
            try
            {
                await using var module = await JS.InvokeAsync<IJSObjectReference>("import",
                    Navigation.ToAbsoluteUri("Components/Pages/TA/JobOfferEmail/TANewJOEmail.razor.js").AbsoluteUri);
                // Read the selected row as well as its salary, since options can share the same salary.
                var optionId = await module.InvokeAsync<int>("getSelectedOptionId", optionSelect);
                var option = compensationOptions.First(item => item.Id == optionId && item.ProposedSalary == salary);
                var url = Navigation.ToAbsoluteUri(JORoutes.TAPartner.JobOfferPdf.TrimStart('/')
                    + $"/{requestedJobOfferId.ToString(CultureInfo.InvariantCulture)}/{salary.ToString(CultureInfo.InvariantCulture)}").AbsoluteUri;
                var pdfBytes = await HtmlToPDFServices.GeneratePdfFromUrlAsync(
                    url, baseUrl: Navigation.BaseUri, readySelector: ".offer-letter[data-ready='true']");
                if (requestedJobOfferId != jobOfferId)
                    return;

                var fileName = $"ProposedOption{option.OptionNumber}.pdf";
                optionAttachments.RemoveAll(file => file.Name == fileName);
                optionAttachments.Add(new FileStreamDto
                {
                    Name = fileName,
                    Content = pdfBytes,
                    SizeInKb = (pdfBytes.Length / 1024d).ToString("N0")
                });
                attachOptionModal?.Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to attach the option PDF for job offer {JobOfferId}.", requestedJobOfferId);
                attachOptionError = "Unable to generate and attach the option PDF. Please try again.";
            }
            finally
            {
                isAttachingOption = false;
            }
        }

        private async Task LoadBenefitsAttachmentAsync()
        {
            isLoadingAttachment = true;
            attachmentError = null;
            benefitsAttachment = null;
            try
            {
                var path = Path.Combine(Environment.WebRootPath, "docs", "benefits.pdf");
                var content = await File.ReadAllBytesAsync(path);
                benefitsAttachment = new FileStreamDto
                {
                    Name = "benefits.pdf",
                    SizeInKb = (content.Length / 1024d).ToString("N0"),
                    Content = content
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load the benefits attachment.");
                attachmentError = "Unable to attach benefits.pdf. Please try again.";
            }
            finally
            {
                isLoadingAttachment = false;
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
            if (messageEditor is null || !isMessageEditorReady)
                return;

            var model = jobOfferEmail;
            var html = await messageEditor.GetHTML();
            if (ReferenceEquals(model, jobOfferEmail))
                model.EmailMessage = html;
        }

        private async Task SaveAttachmentsAsync()
        {
            if (jobOfferEmail.Id <= 0 || string.IsNullOrWhiteSpace(jobOffer?.RefNum))
                throw new InvalidOperationException("A saved email and job offer reference number are required to save attachments.");

            var attachments = new List<FileStreamDto>(optionAttachments);
            if (benefitsAttachment is not null)
                attachments.Insert(0, benefitsAttachment);

            var records = new List<JOHasEmailAttach>();
            var savedPaths = new List<string>();
            try
            {
                foreach (var attachment in attachments)
                {
                    var storedName = $"{Guid.NewGuid():N}_{attachment.Name}";
                    var path = await JOFileService.SaveJobOfferFileAsync(attachment.Content, jobOffer.RefNum, storedName);
                    savedPaths.Add(path);
                    records.Add(new JOHasEmailAttach
                    {
                        JOEmailId = jobOfferEmail.Id,
                        JobOfferId = jobOfferId,
                        FileName = attachment.Name,
                        RelativePath = Path.GetRelativePath(Environment.WebRootPath, path).Replace('\\', '/')
                    });
                }

                // Insert the batch only after every physical file has been saved.
                await JOLetterService.AddRangeJOHasEmailAttach(records);
            }
            catch
            {
                // These paths belong only to this save attempt, never to an earlier draft.
                foreach (var path in savedPaths)
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Unable to clean up attachment {Path} after a failed save.", path);
                    }
                }
                throw;
            }
        }

        private async Task SaveAsync()
        {
            if (hasSavedDraft || !await ValidateEmailAsync())
                return;

            isSaving = true;
            try
            {
                if (!await AlertService.Confirm("Save this job offer email as a draft?", "Save Draft", "Cancel"))
                    return;

                if (jobOfferEmail.Id == 0)
                    jobOfferEmail.Id = await JOLetterService.SaveDraftJobOfferHasEmail(jobOfferEmail);
                else
                    await JOLetterService.UpdateJobOfferHasEmail(jobOfferEmail);

                await SaveAttachmentsAsync();
                hasSavedDraft = true;
                await AlertService.Success("Job offer email draft saved successfully.", "Draft Saved");

                Navigation.NavigateTo($"{JORoutes.TAPartner.JobOfferEmail}/{jobOfferEmail.Id}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to save the job offer email draft.");
                await AlertService.Error("The email and its attachments could not be fully saved. Please retry Save.", "Save Error");
            }
            finally
            {
                isSaving = false;
            }
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
                    FileStreams = benefitsAttachment is null ? [.. optionAttachments] : [benefitsAttachment, .. optionAttachments]
                };

                await EmailService.SendAsync(request);
                await AlertService.Success("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send the job offer test email.");
                await AlertService.Error("The test email could not be sent. Please try again.", "Email Error");
            }
            finally
            {
                isSendingTestMail = false;
            }
        }

        private async Task SubmitForApprovalAsync()
        {
            await ValidateEmailAsync();
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

        private bool IsValidRecipientList(string recipients) =>
            recipients.Split(';').Any(email => !string.IsNullOrWhiteSpace(email))
            && UtilitiesService.IsValidSeriesOfEmail(recipients, ';');
    }
}
