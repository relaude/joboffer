using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JO.BlazorDemoApp.Components.Pages.TA.Tab
{
    public partial class TabJOLetterDiscuss
    {
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
        private List<ChannelTypes> channels = new();
        private List<DiscussSteps> steps = new();
        private List<CandResponse> responses = new();
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

            channels = await DiscussionService.GetChannelTypes();
            steps = await DiscussionService.GetDiscussSteps();
            responses = await DiscussionService.GetCandResponse();
            filteredJOCompanyCompensation = joCompanyCompensation.Where(jo => jo.OptionNumber > 0).ToList();

            var defaultProposal = joCompanyCompensation
                .Where(jo => jo.OptionNumber > 0)
                .OrderBy(jo => jo.OptionNumber)
                .FirstOrDefault();

            discussion.ProposalId ??= defaultProposal?.Id;
            selectedProposedSalary = defaultProposal?.ProposedSalary.GetValueOrDefault() ?? 0m;
            lastSyncedDiscussionProposalId = discussion.ProposalId;

            await GetDiscussions();
            await LoadLetterBody(selectedProposedSalary.GetValueOrDefault());
        }

        private async Task SelectTab(TabName tab)
        {
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
            if (!await AlertService.Confirm())
                return;

            try
            {
                isSaving = true;

                model.JobOfferId = jobOfferId;
                await DiscussionService.SaveDiscussion(model);

                ResetEntries();
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

        private async Task AcceptAndComplete()
        {
            var hasAcceptedDiscussion = vwDiscussions.Any(item =>
                item.StepId == 8 && item.ResponseId == 3);

            if (!hasAcceptedDiscussion)
            {
                await AlertService.Error(
                    "Record a discussion with the Accepted step and Verbally Accepted response before completing this Job Offer.",
                    "Acceptance Discussion Required");
                return;
            }

            var confirmed = await AlertService.Confirm(
                "Tag as Accept and Complete this Job Offer?",
                "Accept & Complete",
                "Cancel");

            if (!confirmed)
                return;

            try
            {
                isCompleting = true;
                await DiscussionService.TagAsAccepted(jobOfferId);
                await AlertService.Success(
                    "The job offer was accepted and completed.",
                    "Job Offer Completed");
            }
            finally
            {
                isCompleting = false;
            }

            // Accepted & Completed, TA Partner, Tag Accepted & Completed
            await ApprovalService.JobOfferActionFlowStatus(jobOfferId, 9, 1, 6, userId);
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
    }
}
