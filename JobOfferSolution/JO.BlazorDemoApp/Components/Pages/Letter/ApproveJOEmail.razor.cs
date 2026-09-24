using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JO.BlazorDemoApp.Components.Pages.Letter
{
    public partial class ApproveJOEmail
    {
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private ILogger<ApproveJOEmail> Logger { get; set; } = default!;

        [Parameter, EditorRequired] public int EmailId { get; set; }
        [Parameter, EditorRequired] public int RoleId { get; set; }

        private JobOfferHasEmail jobOfferEmail = new();
        private List<JOHasEmailAttach> attachments = new();
        private bool isLoading = true;
        private bool isApproving;
        private bool isSendingBack;
        private string? loadError;
        private int userId;
        private bool CanApprove => !isLoading && !isApproving && !isSendingBack && loadError is null && jobOfferEmail.StatusId == 2;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            loadError = null;
            jobOfferEmail = new();
            attachments.Clear();
            try
            {
                userId = await AccountService.GetJobOfferUserId();
                var email = await JOLetterService.GetJobOfferHasEmail(EmailId);
                if (email is null || email.StatusId is not (2 or 3))
                {
                    loadError = "This email is not available for approval.";
                    return;
                }
                jobOfferEmail = email;
                attachments = await JOLetterService.GetJOHasEmailAttach(EmailId);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load email {EmailId} for approval.", EmailId);
                loadError = "Unable to load this email. Please reload the page and try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SendBackAsync()
        {
            if (!CanApprove)
                return;

            isSendingBack = true;
            var email = jobOfferEmail;
            var sendingBackRoleId = RoleId;
            var sendingBackUserId = userId;
            try
            {
                string? remarks;
                while (true)
                {
                    remarks = await AlertService.ConfirmRemarks(title: "Send Back — Remarks required");
                    if (remarks is null)
                        return;
                    if (!string.IsNullOrWhiteSpace(remarks))
                        break;

                    await AlertService.Error("Please enter remarks before sending back this email.", "Remarks Required");
                }

                var previousStatus = email.StatusId;
                var previousModifiedAt = email.ModifiedAt;
                email.StatusId = 1;
                email.ModifiedAt = DateTime.Now;
                try
                {
                    await JOLetterService.UpdateJobOfferHasEmail(email, sendingBackRoleId, 13, sendingBackUserId, remarks.Trim());
                }
                catch
                {
                    email.StatusId = previousStatus;
                    email.ModifiedAt = previousModifiedAt;
                    throw;
                }

                await AlertService.Success("Job offer email sent back successfully.", "Sent Back");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to send back job offer email {EmailId}.", email.Id);
                await AlertService.Error("The email could not be sent back. Please reload the page and try again.", "Send Back Error");
            }
            finally
            {
                isSendingBack = false;
            }
        }

        private async Task ApproveAsync()
        {
            if (!CanApprove)
                return;
            isApproving = true;
            var approvingEmailId = EmailId;
            try
            {
                if (!await AlertService.Confirm("Approve this job offer email?", "Approve", "Cancel"))
                    return;
                //await JOLetterService.AproveJobOfferHasEmail(approvingEmailId);
                jobOfferEmail.StatusId = 3;
                await JOLetterService.UpdateJobOfferHasEmail(jobOfferEmail, RoleId, 12, userId, jobOfferEmail.Subject);

                await AlertService.Success("Job offer email approved successfully.", "Approved");
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
    }
}
