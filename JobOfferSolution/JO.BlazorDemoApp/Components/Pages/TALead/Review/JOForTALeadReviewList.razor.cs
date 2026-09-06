using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.Review
{
    public partial class JOForTALeadReviewList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;

        private static readonly int[] ForReviewWorkFlowIds = [3]; //For TA Lead Review
        private static readonly int[] SendBackWorkFlowIds = [10]; //Send Back
        private static readonly int[] ReviewedWorkFlowIds = [4, 5, 6, 7, 8, 9, 13];

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> eligibleJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private int userId;
        private int total;
        private int countForReview;
        private int countSendBack;
        private int countReviewed;

        protected override async Task OnInitializedAsync()
        {
            userId = await AccountService.GetJobOfferUserId();

            joDboxCandidates = await JODetailsService.GetTALeadForReviewVwJODboxCandidates(userId);
            eligibleJODboxCandidates = joDboxCandidates
                .Where(jo => jo.WorkFlowId is int id
                        && (ForReviewWorkFlowIds.Contains(id)
                            || SendBackWorkFlowIds.Contains(id)
                            || ReviewedWorkFlowIds.Contains(id)))
                .ToList();

            total = eligibleJODboxCandidates.Count;
            countForReview = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && ForReviewWorkFlowIds.Contains(id));
            countSendBack = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && SendBackWorkFlowIds.Contains(id));
            countReviewed = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && ReviewedWorkFlowIds.Contains(id));

            FilterByWorkFlow(null);
        }

        private void FilterByWorkFlow(int[]? workFlowIds)
        {
            filteredJODboxCandidates = workFlowIds is null
                ? eligibleJODboxCandidates.ToList()
                : eligibleJODboxCandidates
                    .Where(jo => jo.WorkFlowId is int id && workFlowIds.Contains(id))
                    .ToList();
        }
    }
}
