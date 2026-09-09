using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.PEHead.Approval
{
    public partial class JOForPEHApprovalList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;

        private static readonly int[] ForApprovalWorkFlowIds = [4];
        private static readonly int[] SendBackWorkFlowIds = [10];
        private static readonly int[] ApprovedWorkFlowIds = [8, 9];

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> eligibleJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private int total;
        private int countForApproval;
        private int countSendBack;
        private int countApproved;

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            eligibleJODboxCandidates = joDboxCandidates
                .Where(jo => jo.WorkFlowId is int id
                    && (((jo.OfferRangeId == 2 || jo.OfferRangeId == 3)
                            && ForApprovalWorkFlowIds.Contains(id))
                        || SendBackWorkFlowIds.Contains(id)
                        || ApprovedWorkFlowIds.Contains(id)))
                .ToList();

            total = eligibleJODboxCandidates.Count;
            countForApproval = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && ForApprovalWorkFlowIds.Contains(id));
            countSendBack = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && SendBackWorkFlowIds.Contains(id));
            countApproved = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId is int id && ApprovedWorkFlowIds.Contains(id));

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
