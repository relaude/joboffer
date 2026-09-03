using Humanizer;
using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JO.BlazorDemoApp.Components.Pages.TA.TabsDiscussion
{
    public partial class TabJOLetterDiscuss
    {
        private const int TabTransitionDelayMilliseconds = 100;

        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IDiscussionService DiscussionService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
        [Inject] private IEmailService EmailService { get; set; } = default!;
        [Inject] private IApprovalService ApprovalService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }

        private TabName activeTab = TabName.JODetails;
        private int userId;

        private JobOffers jobOffer = new();
        private VwJODboxCandidates vwJODboxCandidates = new();
        private VwDboxCandidates candidate = new();
        private VwSalaryBands vwSalaryBand = new();
        private JOAnalysis joAnalysis = new();

        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        private List<CompanyCompensation> companyCompensation = new();
        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOCompanyCompensationItems> joCompanyCompensationItems = new();
        private List<CompenItemCategoryDto> compenItemCategoryDto = new();

        //Discussion
        private DiscussionDto discussion = new()
        {
            DiscussAt = DateTime.Now,
            StepId = 1,
            ChannelId = 1,
            ResponseId = 1
        };
        //private List<ChannelTypes> channels = new();
        //private List<DiscussSteps> steps = new();
        private List<DiscussionStatus> discussionStatus = new();
        private List<JODeclineReason> declineReason = new();
        //private List<CandResponse> responses = new();
        private List<JOCompanyCompensation> filteredJOCompanyCompensation = new();
        private List<VwDiscussions> vwDiscussions = new();
        private bool isSaving = false;
        private bool isCompleting;

        //Letter
        private List<JOItemLetter> joItemLetters = new();
        private string letterBody = string.Empty;
        private string emailSubject = string.Empty;
        private string testEmailRecipient = string.Empty;
        private bool isSending;
        private decimal? selectedProposedSalary;
        private int? lastSyncedDiscussionProposalId;

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();

            jobOffer = await DiscussionService.GetJobOffer(jobOfferId);
            vwJODboxCandidates = await DiscussionService.GetVwJODboxCandidates(jobOfferId);
            candidate = await DiscussionService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwCompanyCompensationItems = await CompensationService.GetVwCompanyCompensationItems(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());

            vwSalaryBand = await CompensationService.GetVwSalaryBand(jobOffer.CompanyId.GetValueOrDefault(), candidate.CSGId.GetValueOrDefault());
            joAnalysis = await CompensationService.GetJOAnalysis(jobOfferId);
            companyCompensation = await CompensationService.GetCompanyCompensation(jobOffer.CompanyId.GetValueOrDefault());
            joCompanyCompensation = await CompensationService.GetJOCompanyCompensation(jobOfferId);
            joCompanyCompensationItems = await CompensationService.GetJOCmpnyCompensationItems(jobOfferId);
            compenItemCategoryDto = await CompensationService.SetUpCompenItemCategoryDto();

            //channels = await DiscussionService.GetChannelTypes();
            //steps = await DiscussionService.GetDiscussSteps();

            discussionStatus = await DiscussionService.GetDiscussionStatus();
            declineReason = await DiscussionService.GetJODeclineReason();

            if (!discussion.StatusId.HasValue
                || !discussionStatus.Any(status => status.Id == discussion.StatusId.Value))
            {
                discussion.StatusId = discussionStatus.FirstOrDefault()?.Id;
            }

            filteredJOCompanyCompensation = joCompanyCompensation.Where(jo => jo.OptionNumber > 0).ToList();

            var availableProposals = joCompanyCompensation
                .Where(jo => jo.OptionNumber > 0 && jo.Declined == false)
                .OrderBy(jo => jo.OptionNumber)
                .ToList();

            var selectedProposal = availableProposals
                .FirstOrDefault(jo => jo.Id == discussion.ProposalId)
                ?? availableProposals.FirstOrDefault();

            discussion.ProposalId = selectedProposal?.Id;
            selectedProposedSalary = selectedProposal?.ProposedSalary.GetValueOrDefault() ?? 0m;
            lastSyncedDiscussionProposalId = discussion.ProposalId;

            await GetDiscussions();
            await LoadLetterBody(selectedProposedSalary.GetValueOrDefault());
        }

        private async Task SelectTab(TabName tab)
        {
            await Task.Delay(TabTransitionDelayMilliseconds);

            if (tab == TabName.Letter)
                await SyncLetterOptionFromDiscussion();

            activeTab = tab;
        }

        private enum TabName
        {
            JODetails,
            Discussion,
            Letter,
            ActionLogs
        }

        private async Task SaveDiscussion(DiscussionDto model)
        {
            var errors = new List<string>();

            if (!model.StatusId.HasValue)
                errors.Add("Status is required.");

            if (!model.ProposalId.HasValue)
                errors.Add("Proposal is required.");

            if (!model.DiscussAt.HasValue)
                errors.Add("Discussion date is required.");

            if (string.IsNullOrWhiteSpace(model.Comments))
                errors.Add("Discussion Notes are required.");

            if (string.IsNullOrWhiteSpace(model.FeedBack))
                errors.Add("Feedback is required.");

            if (model.StatusId == 4 && !model.DeclineReasonId.HasValue)
                errors.Add("Decline reason is required for a declined offer.");

            if (model.StatusId == 4
                && model.DeclineReasonId == 5
                && string.IsNullOrWhiteSpace(model.DeclineRemarks))
            {
                errors.Add("Decline remarks are required when the decline reason is Others.");
            }

            if (errors.Any())
            {
                await AlertService.Errors(errors, "Required Fields");
                return;
            }

            if (!await AlertService.Confirm())
                return;

            try
            {
                isSaving = true;

                model.JobOfferId = jobOfferId;
                await DiscussionService.SaveDiscussion(model);

                //Accepted Offer || Declined
                if (model.StatusId == 3 || model.StatusId == 4)
                {
                    joCompanyCompensation = await CompensationService.GetJOCompanyCompensation(jobOfferId);
                    filteredJOCompanyCompensation = joCompanyCompensation
                        .Where(jo => jo.OptionNumber > 0)
                        .ToList();
                }

                //ResetEntries();
                await GetDiscussions();
            }
            finally
            {
                isSaving = false;
            }
        }

        private async Task GetDiscussions()
        {
            vwDiscussions = await DiscussionService.GetDiscussions(jobOfferId);
        }

        private bool HasAcceptedOffer => vwDiscussions.Any(item => item.StatusId == 3);
        private bool HasDeclinedOffer => vwDiscussions.Any(item => item.StatusId == 4);
        private bool HasForNegotiation => vwDiscussions.Any(item => item.StatusId == 5);

        private async Task AcceptAndComplete()
        {
            //var confirmed = await AlertService.Confirm(
            //    "Tag as Accept and Complete this Job Offer?",
            //    "Accept & Complete",
            //    "Cancel");

            //if (!confirmed)
            //    return;

            string promptRemarks = await AlertService.ConfirmRemarks("", "Tag as Accept and Complete this Job Offer?");
            if (string.IsNullOrEmpty(promptRemarks)) return;

            try
            {
                isCompleting = true;

                await DiscussionService.TagAsAccepted(jobOfferId);

                // Accepted & Completed, TA Partner, Tag Accepted & Completed
                await ApprovalService.JobOfferActionFlowStatus(jobOfferId, 9, 1, 6, userId, promptRemarks);

                await AlertService.Success(
                    "The job offer was accepted and completed.",
                    "Job Offer Completed");
            }
            finally
            {
                isCompleting = false;
            }

            Navigation.NavigateTo($"{JORoutes.TA.JobOfferComplete}/{jobOfferId}");
        }

        private async Task Decline()
        {
            //var confirmed = await AlertService.Confirm(
            //    "Tag as Declined this Job Offer?",
            //    "Declined",
            //    "Cancel");

            //if (!confirmed)
            //    return;

            string promptRemarks = await AlertService.ConfirmRemarks("","Tag as Declined this Job Offer?");
            if (string.IsNullOrEmpty(promptRemarks)) return;

            try
            {
                isCompleting = true;
                await DiscussionService.TagAsAccepted(jobOfferId);

                // Declined, TA Partner, Tag Decline
                await ApprovalService.JobOfferActionFlowStatus(jobOfferId, 12, 1, 9, userId, promptRemarks);

                await AlertService.Success(
                    "The job offer was declined.",
                    "Job Offer Declined");
            }
            finally
            {
                isCompleting = false;
            }

            Navigation.NavigateTo($"{JORoutes.TA.JobOfferComplete}/{jobOfferId}");
        }

        private void ResetEntries()
        {
            discussion.DiscussAt = DateTime.Now;
            discussion.StepId = 1;
            discussion.ChannelId = 1;
            discussion.ResponseId = 1;
            discussion.Comments = "";
            discussion.FeedBack = "";
        }

        private async Task LoadLetterBody(decimal proposedSalary)
        {
            joItemLetters = await JOLetterService.GetJOItemLetter(
                jobOffer.CmpnyCmpnstnId.GetValueOrDefault());

            JOLetterService.UpdateItemLetterPlaceHolder(
                joItemLetters,
                candidate,
                proposedSalary);

            letterBody = string.Join(
                "<p><br></p>",
                joItemLetters
                    .Where(item => !string.IsNullOrWhiteSpace(item.MessageBody))
                    .Select(item => item.MessageBody));
        }

        private async Task SyncLetterOptionFromDiscussion()
        {
            await Task.Delay(TabTransitionDelayMilliseconds);

            if (discussion.ProposalId == lastSyncedDiscussionProposalId)
                return;

            lastSyncedDiscussionProposalId = discussion.ProposalId;
            var proposal = filteredJOCompanyCompensation
                .FirstOrDefault(item => item.Id == discussion.ProposalId);

            if (proposal is null)
                return;

            selectedProposedSalary = proposal.ProposedSalary.GetValueOrDefault();
            await LoadLetterBody(selectedProposedSalary.Value);
        }

        private async Task ChangeLetterCompensationOption(decimal proposedSalary)
        {
            selectedProposedSalary = proposedSalary;
            await LoadLetterBody(proposedSalary);
        }

        private async Task SendTestMail(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Subject))
            {
                await AlertService.Error(
                    "Enter an email subject before sending the test email.",
                    "Email Subject Required");
                return;
            }

            if (!UtilitiesService.IsValidEmail(request.To))
            {
                await AlertService.Error(
                    "Enter a valid test email address.",
                    "Invalid Email Address");
                return;
            }

            if (!await AlertService.Confirm(
                $"Send a test email to {request.To}?",
                "Send Test Email",
                "Cancel"))
            {
                return;
            }

            try
            {
                isSending = true;
                await EmailService.SendAsync(request);
                await AlertService.Success("The test email was sent.", "Test Email Sent");
            }
            finally
            {
                isSending = false;
            }
        }

        private async Task SendEmail(EmailRequest request)
        {
            if (!await AlertService.Confirm(
                $"Send the job offer to {request.To}?",
                "Send Job Offer",
                "Cancel"))
            {
                return;
            }

            try
            {
                isSending = true;
                await EmailService.SendAsync(request);
                await AlertService.Success("The job offer was sent.", "Email Sent");
            }
            finally
            {
                isSending = false;
            }
        }

        private async Task ForNegotiation()
        {
            int numProposal = await AlertService.ConfirmProposalNumber("1");

            if (numProposal == 0) return;

            await DiscussionService.ForNegotiation(jobOffer,
                joAnalysis,
                candidate,
                joCompanyCompensation,
                numProposal,
                userId);

            // For Negotiation, TA Partner, Tag For Negotiation
            await ApprovalService.JobOfferActionFlowStatus(jobOfferId, 11, 1, 8, userId, $"Tag For Negotiation ({numProposal} Proposal/s)");

            Navigation.NavigateTo($"{JORoutes.TA.ForNegotiation}/{jobOfferId}");
        }
    }
}
