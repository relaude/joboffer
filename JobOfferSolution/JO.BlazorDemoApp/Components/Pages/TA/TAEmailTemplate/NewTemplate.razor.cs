using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.DataModel.DTOs;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using WYSIWYGTextEditor;

namespace JO.BlazorDemoApp.Components.Pages.TA.TAEmailTemplate
{
    public partial class NewTemplate
    {
        [Inject] private IEmailTemplateService EmailTemplateService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private ILogger<NewTemplate> Logger { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private EmailTemplate emailTemplate = new();
        private TextEditor? messageEditor;
        private List<JOWorkFlowStatus> workFlowStatuses = [];
        private List<VwJOUserRoles> recipientRoles = [];
        private readonly HashSet<int> selectedRecipientRoleIds = [];
        private bool isProcessingSave;
        private string testEmailRecipient = string.Empty;
        private bool isSendingTestMail;
        private int userId;

        protected override async Task OnInitializedAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            workFlowStatuses = await EmailTemplateService.GetFilteredJOWorkFlowStatus();
            recipientRoles = await EmailTemplateService.GetVwJOUserRoles();
        }

        private void UpdateRecipientRole(int roleId, bool isChecked)
        {
            if (isChecked)
            {
                selectedRecipientRoleIds.Add(roleId);
            }
            else
            {
                selectedRecipientRoleIds.Remove(roleId);
            }
        }

        private async Task TestMailAsync()
        {
            if (isSendingTestMail)
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

            if (isSendingTestMail)
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
            if (isProcessingSave)
            {
                return;
            }

            isProcessingSave = true;
            try
            {
                await UpdateEmailMessageAsync();
                var messageText = messageEditor is null ? string.Empty : await messageEditor.GetText();
                var errors = new List<string>();

                if (!emailTemplate.WorkFlowId.HasValue
                    || !workFlowStatuses.Any(workflow => workflow.Id == emailTemplate.WorkFlowId.Value))
                {
                    errors.Add("Job Offer WorkFlow is required.");
                }

                if (string.IsNullOrWhiteSpace(emailTemplate.EmailSubject))
                {
                    errors.Add("Email Subject is required.");
                }

                if (string.IsNullOrWhiteSpace(messageText))
                {
                    errors.Add("Email Message is required.");
                }

                if (selectedRecipientRoleIds.Count == 0 && string.IsNullOrWhiteSpace(emailTemplate.OtherRecipient))
                {
                    errors.Add("Select at least one recipient role or enter email addresses in Others.");
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

                // Persistence can be added here when the template service exposes a save operation.
                emailTemplate.CreatedBy = userId;
                int templateId = await EmailTemplateService.CreateEmailTemplate(emailTemplate, selectedRecipientRoleIds);
                await AlertService.Success("Email template updated successfully.");

                Navigation.NavigateTo($"{JORoutes.TAPartner.EmailTemplate}/{templateId}");
            }
            finally
            {
                isProcessingSave = false;
            }
        }

        private async Task UpdateEmailMessageAsync()
        {
            if (messageEditor is not null)
            {
                emailTemplate.EmailMessage = await messageEditor.GetHTML();
            }
        }
    }
}
