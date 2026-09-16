using JO.DataModel.Entity;
using JO.DataModel.DTOs;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.Candidate
{
    public partial class EditEmailTemplate
    {
        [Inject] private IEmailTemplateService EmailTemplateService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private ILogger<EditEmailTemplate> Logger { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;

        [Parameter] public int templateId { get; set; }

        private bool isLoading = true;
        private bool templateFound;
        private bool loadMessageContent;
        private bool IsActive
        {
            get => emailTemplate.IsActive == true;
            set => emailTemplate.IsActive = value;
        }

        private CandidateEmailTemplate emailTemplate = new();
        private TextEditor? messageEditor;
        private bool isProcessingSave;
        private string testEmailRecipient = string.Empty;
        private bool isSendingTestMail;
        private int userId;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            templateFound = false;
            loadMessageContent = false;
            messageEditor = null;
            userId = await AccountService.GetJobOfferUserId();
            var template = await EmailTemplateService.GetCandidateEmailTemplate(templateId);
            if (template is not null)
            {
                emailTemplate = template;
                templateFound = true;
                loadMessageContent = true;
            }
            isLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!loadMessageContent || messageEditor is null)
            {
                return;
            }

            // Match the existing letter editor's initialization before loading HTML.
            await Task.Delay(500);
            await messageEditor.LoadHTMLContent(emailTemplate.EmailMessage ?? string.Empty);
            loadMessageContent = false;
            StateHasChanged();
        }

        private async Task TestMailAsync()
        {
            if (isSendingTestMail || isLoading || !templateFound || loadMessageContent)
                return;

            await UpdateEmailMessageAsync();
            var messageText = messageEditor is null ? string.Empty : await messageEditor.GetText();
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(emailTemplate.EmailSubject))
            {
                errors.Add("Email Subject is required.");
            }

            if (string.IsNullOrWhiteSpace(messageText))
            {
                errors.Add("Email Message is required.");
            }

            if (string.IsNullOrWhiteSpace(testEmailRecipient))
            {
                errors.Add("Test Recipient Email Address is required.");
            }
            else if (!UtilitiesService.IsValidEmail(testEmailRecipient.Trim()))
            {
                errors.Add("Test Recipient Email Address must be a valid email address.");
            }

            if (errors.Count > 0)
            {
                await AlertService.Errors(errors, "Validation Errors");
                return;
            }

            if (isSendingTestMail || isLoading || !templateFound || loadMessageContent)
                return;

            isSendingTestMail = true;
            try
            {
                var request = new EmailRequest
                {
                    To = testEmailRecipient.Trim(),
                    Subject = emailTemplate.EmailSubject!.Trim(),
                    Body = emailTemplate.EmailMessage ?? string.Empty
                };

                await EmailService.SendAsync(request);
                await AlertService.Success("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send the template test email.");
                await AlertService.Errors(
                    new List<string> { "The test email could not be sent. Please try again." },
                    "Email Error");
            }
            finally
            {
                isSendingTestMail = false;
            }
        }

        private async Task SaveTemplateAsync()
        {
            if (isProcessingSave || isLoading || !templateFound || loadMessageContent)
            {
                return;
            }

            isProcessingSave = true;
            try
            {
                await UpdateEmailMessageAsync();
                var messageText = messageEditor is null ? string.Empty : await messageEditor.GetText();
                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(emailTemplate.EmailSubject))
                {
                    errors.Add("Email Subject is required.");
                }

                if (string.IsNullOrWhiteSpace(messageText))
                {
                    errors.Add("Email Message is required.");
                }

                if (string.IsNullOrWhiteSpace(emailTemplate.OtherRecipient))
                {
                    errors.Add("Others TO Recipients is required.");
                }

                if (!string.IsNullOrWhiteSpace(emailTemplate.OtherRecipient)
                    && !UtilitiesService.IsValidSeriesOfEmail(emailTemplate.OtherRecipient))
                {
                    errors.Add("Others must contain valid email addresses separated by semicolons (;).");
                }

                if (!string.IsNullOrWhiteSpace(emailTemplate.CCRecipient)
                    && !UtilitiesService.IsValidSeriesOfEmail(emailTemplate.CCRecipient))
                {
                    errors.Add("CC Recipients must contain valid email addresses separated by semicolons (;).");
                }

                if (errors.Count > 0)
                {
                    await AlertService.Errors(errors, "Validation Errors");
                    return;
                }

                if (!await AlertService.Confirm("Save email template?", "Save Template", "Cancel"))
                {
                    return;
                }

                emailTemplate.ModifiedBy = userId;
                await EmailTemplateService.UpdateCandidateEmailTemplate(emailTemplate);
                await AlertService.Success("Email template updated successfully.");
            }
            finally
            {
                isProcessingSave = false;
            }
        }

        private async Task UpdateEmailMessageAsync()
        {
            if (messageEditor is not null && !loadMessageContent)
            {
                emailTemplate.EmailMessage = await messageEditor.GetHTML();
            }
        }
    }
}
