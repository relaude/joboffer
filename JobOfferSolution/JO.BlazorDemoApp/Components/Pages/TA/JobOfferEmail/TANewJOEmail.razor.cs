using JO.DataModel.Entity;
using JO.DataModel.DTOs;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class TANewJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private ILogger<TANewJOEmail> Logger { get; set; } = default!;

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
        private bool IsActionDisabled => isLoadingMessage || !isMessageEditorReady || isValidating || isSendingTestMail || isSaving;

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

            try
            {
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

        private async Task SaveAsync()
        {
            if (hasSavedDraft || !await ValidateEmailAsync())
                return;

            isSaving = true;
            try
            {
                if (!await AlertService.Confirm("Save this job offer email as a draft?", "Save Draft", "Cancel"))
                    return;

                jobOfferEmail.Id = await JOLetterService.SaveDraftJobOfferHasEmail(jobOfferEmail);
                hasSavedDraft = true;
                await AlertService.Success("Job offer email draft saved successfully.", "Draft Saved");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to save the job offer email draft.");
                await AlertService.Error("The draft could not be saved. Please try again.", "Save Error");
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
                    Body = jobOfferEmail.EmailMessage ?? string.Empty
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
