using JO.DataModel.Entity;
using JO.DataModel.DTOs;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOfferEmail
{
    public partial class TAEditJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private ILogger<TAEditJOEmail> Logger { get; set; } = default!;

        [Parameter] public int emailId { get; set; }

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
        private string? loadError;
        private bool IsActionDisabled => isLoadingMessage || !isMessageEditorReady || isValidating || isSendingTestMail || isSaving || loadError is not null;

        protected override async Task OnParametersSetAsync()
        {
            isLoadingMessage = true;
            loadMessageContent = false;
            isMessageEditorReady = false;
            messageEditor = null;
            loadError = null;
            jobOfferEmail = new();
            try
            {
                userId = await AccountService.GetJobOfferUserId();
                var email = await JOLetterService.GetJobOfferHasEmail(emailId);
                if (email is null)
                {
                    loadError = "The job offer email was not found.";
                    return;
                }
                jobOfferEmail = email;
                jobOfferEmail.ModifiedBy = userId;
                loadMessageContent = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load job offer email {EmailId}.", emailId);
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
            if (messageEditor is null || !isMessageEditorReady)
                return;

            var model = jobOfferEmail;
            var html = await messageEditor.GetHTML();
            if (ReferenceEquals(model, jobOfferEmail))
                model.EmailMessage = html;
        }

        private Task SaveAsync() => UpdateEmailAsync(1);

        private async Task UpdateEmailAsync(int statusId)
        {
            if (!await ValidateEmailAsync())
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
                await JOLetterService.UpdateJobOfferHasEmail(jobOfferEmail);
                await AlertService.Success(
                    submit ? "Job offer email submitted for approval." : "Job offer email draft updated successfully.",
                    submit ? "Submitted for Approval" : "Draft Saved");
            }
            catch (Exception ex)
            {
                jobOfferEmail.StatusId = previousStatus;
                Logger.LogError(ex, "Failed to update job offer email {EmailId}.", emailId);
                await AlertService.Error("The email could not be updated. Please try again.", "Update Error");
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

        private Task SubmitForApprovalAsync() => UpdateEmailAsync(2);

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

