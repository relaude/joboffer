using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.President.Aproval
{
    public partial class JOForPresidentApprovalList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;

        private const int ForApprovalWorkFlowId = 7;
        private const int SendBackWorkFlowId = 10;
        private const int ApprovedWorkFlowId = 8;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> eligibleJODboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        private int total;
        private int countForApproval;
        private int countSendBack;
        private int countApproved;
        private int?[] approvedIDs = { 8, 9 };

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            eligibleJODboxCandidates = joDboxCandidates
                .Where(jo => jo.OfferRangeId == 3
                    && (jo.WorkFlowId == ForApprovalWorkFlowId
                        || jo.WorkFlowId == SendBackWorkFlowId
                        || approvedIDs.Contains(jo.WorkFlowId)))
                .ToList();

            total = eligibleJODboxCandidates.Count;
            countForApproval = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId == ForApprovalWorkFlowId);
            countSendBack = eligibleJODboxCandidates.Count(jo => jo.WorkFlowId == SendBackWorkFlowId);
            countApproved = eligibleJODboxCandidates.Count(jo => approvedIDs.Contains(jo.WorkFlowId));

            FilterByWorkFlow(null);
        }

        private void FilterByWorkFlow(int? workFlowId)
        {
            filteredJODboxCandidates = workFlowId switch
            {
                null => eligibleJODboxCandidates.ToList(),
                ApprovedWorkFlowId => eligibleJODboxCandidates
                    .Where(jo => approvedIDs.Contains(jo.WorkFlowId))
                    .ToList(),
                _ => eligibleJODboxCandidates
                    .Where(jo => jo.WorkFlowId == workFlowId)
                    .ToList()
            };
        }
    }
}
