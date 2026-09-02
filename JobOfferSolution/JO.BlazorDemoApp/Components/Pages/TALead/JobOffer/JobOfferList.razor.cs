using JO.BlazorDemoApp.Components.Pages.JobOffer;
using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.TALead.JobOffer
{
    public partial class JobOfferList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> trackableJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private static readonly int?[] ForApprovalWorkFlowIds = { 4, 5, 6, 7 };

        private int total = 0;
        private int countForReview = 0;
        private int countReviewed = 0;
        private int countSendBack = 0;
        private int countForApproval = 0;
        private int countForDiscussion = 0;
        private int countApproved = 0;
        private int countAcccepted = 0;

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
            countReviewed = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 4);
            countSendBack = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 10);
            countForApproval = trackableJODboxCandidates.Count(jo => ForApprovalWorkFlowIds.Contains(jo.WorkFlowId));
            countForDiscussion = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 8);
            countApproved = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 8);
            countAcccepted = trackableJODboxCandidates.Count(jo => jo.WorkFlowId == 9);
        }

        private void FilterByWorkFlow(int? workFlowId)
        {
            filteredJODboxCandidates = workFlowId switch
            {
                null => trackableJODboxCandidates.ToList(),
                JobOfferKpiBoxes.ForApprovalFilter => trackableJODboxCandidates
                    .Where(jo => ForApprovalWorkFlowIds.Contains(jo.WorkFlowId))
                    .ToList(),
                _ => trackableJODboxCandidates
                    .Where(jo => jo.WorkFlowId == workFlowId)
                    .ToList()
            };
        }


    }
}
