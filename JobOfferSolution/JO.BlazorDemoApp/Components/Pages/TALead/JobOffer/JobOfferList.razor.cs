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
        private List<VwTALeadDboxCandidates> vwTALeadDboxCandidates = new();
        private List<VwJODboxCandidates> trackableJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private static readonly int[] ForReviewWorkFlowIds = [14];
        private static readonly int[] ReviewedWorkFlowIds = [6];
        private static readonly int[] SendBackWorkFlowIds = [10];
        private static readonly int[] ForApprovalWorkFlowIds = [5, 6, 7, 13];
        private static readonly int[] ForDiscussionWorkFlowIds = [8];
        private static readonly int[] ApprovedWorkFlowIds = [0];
        private static readonly int[] AcceptedWorkFlowIds = [9];
        private static readonly int[] ForNegotiationWorkFlowIds = [11];
        private static readonly int[] DeclinedWorkFlowIds = [12];

        private int total = 0;
        private int countForReview = 0;
        private int countReviewed = 0;
        private int countSendBack = 0;
        private int countForApproval = 0;
        private int countForDiscussion = 0;
        private int countApproved = 0;
        private int countAcccepted = 0;
        private int countForNegotiation = 0;
        private int countDeclined = 0;

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            vwTALeadDboxCandidates = await JODetailsService.GetVwTALeadDboxCandidates();

            List<string?> candidateIds = vwTALeadDboxCandidates.Select(jo => jo.DboxRefNum).ToList();
            trackableJODboxCandidates = joDboxCandidates
                .Where(jo => candidateIds.Contains(jo.DboxRefNum) && jo.WorkFlowId > 1)
                .ToList();
            filteredJODboxCandidates = trackableJODboxCandidates.ToList();
            SetUpCountStatus();
        }

        private void SetUpCountStatus()
        {
            total = trackableJODboxCandidates.Count;
            countForReview = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ForReviewWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countReviewed = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ReviewedWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countSendBack = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && SendBackWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countForApproval = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ForApprovalWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countForDiscussion = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ForDiscussionWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countApproved = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ApprovedWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countAcccepted = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && AcceptedWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countForNegotiation = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && ForNegotiationWorkFlowIds.Contains(jo.WorkFlowId.Value));
            countDeclined = trackableJODboxCandidates.Count(jo => jo.WorkFlowId.HasValue && DeclinedWorkFlowIds.Contains(jo.WorkFlowId.Value));
        }

        private void FilterByWorkFlow(int[]? workFlowIds)
        {
            filteredJODboxCandidates = workFlowIds is null
                ? trackableJODboxCandidates.ToList()
                : trackableJODboxCandidates
                    .Where(jo => jo.WorkFlowId.HasValue && workFlowIds.Contains(jo.WorkFlowId.Value))
                    .ToList();
        }
    }
}
