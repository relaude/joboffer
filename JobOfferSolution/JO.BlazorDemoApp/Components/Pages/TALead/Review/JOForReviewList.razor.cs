using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.Review
{
    public partial class JOForReviewList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> trackableJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private int total = 0;
        private int countForReview = 0;
        private int countReviewed = 0;
        private int countSendBack = 0;
        private int?[] reviewedIDs = { 4, 5, 6, 7, 8, 9 };

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            trackableJODboxCandidates = joDboxCandidates
                .Where(jo => jo.WorkFlowId > 1)
                .ToList();
            filteredJODboxCandidates = trackableJODboxCandidates.ToList();
            SetUpCountStatus();
        }

        private void SetUpCountStatus()
        {
            total = trackableJODboxCandidates.Count;
            countForReview = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 3);
            countReviewed = trackableJODboxCandidates.Count(jo => reviewedIDs.Contains(jo.WorkFlowId));
            countSendBack = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 10);
        }

        private void FilterByWorkFlow(int? workFlowId)
        {
            filteredJODboxCandidates = workFlowId switch
            {
                null => trackableJODboxCandidates.ToList(),
                4 => trackableJODboxCandidates
                    .Where(jo => reviewedIDs.Contains(jo.WorkFlowId))
                    .ToList(),
                _ => trackableJODboxCandidates
                    .Where(jo => jo.WorkFlowId == workFlowId)
                    .ToList()
            };
        }
    }
}
