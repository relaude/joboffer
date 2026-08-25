using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TA.Discussion
{
    public partial class JobOfferDiscussion
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IDiscussionService DiscussionService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }
        private int userId;

        private JobOffers jobOffer = new();
        private VwJODboxCandidates vwJODboxCandidates = new();
        private VwDboxCandidates candidate = new();

        private DiscussionDto discussion = new()
        {
            DiscussAt = DateTime.Now,
            StepId = 1,
            ChannelId = 1,
            ResponseId = 1
        };

        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        private List<ChannelTypes> channels = new();
        private List<DiscussSteps> steps = new();
        private List<CandResponse> responses = new();
        private List<VwDiscussions> vwDiscussions = new();
        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOCompanyCompensation> filteredJOCompanyCompensation = new();

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            
            jobOffer = await DiscussionService.GetJobOffer(jobOfferId);
            vwJODboxCandidates = await DiscussionService.GetVwJODboxCandidates(jobOfferId);

            joCompanyCompensation = await DiscussionService.GetJOCompanyCompensation(jobOfferId);
            filteredJOCompanyCompensation = joCompanyCompensation.Where(jo=> jo.OptionNumber > 0).ToList();

            candidate = await DiscussionService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwCompanyCompensationItems = await CompensationService.GetVwCompanyCompensationItems(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());

            channels = await DiscussionService.GetChannelTypes();
            steps = await DiscussionService.GetDiscussSteps();
            responses = await DiscussionService.GetCandResponse();

            await GetDiscussions();
        }

        private async Task GetDiscussions()
        {
            vwDiscussions = await DiscussionService.GetDiscussions(jobOfferId);
        }

        private async Task SaveDiscussion()
        {
            if (!await AlertService.Confirm()) return;

            discussion.JobOfferId = jobOfferId;
            await DiscussionService.SaveDiscussion(discussion);

            ResetEntries();
            await GetDiscussions();
        }

        private void ResetEntries()
        {
            discussion.ProposalId = filteredJOCompanyCompensation.FirstOrDefault().Id;
            discussion.DiscussAt = DateTime.Now;
            discussion.StepId = 1;
            discussion.ChannelId = 1;
            discussion.ResponseId = 1;
            discussion.Comments = "";
            discussion.FeedBack = "";
        }
    }
}
