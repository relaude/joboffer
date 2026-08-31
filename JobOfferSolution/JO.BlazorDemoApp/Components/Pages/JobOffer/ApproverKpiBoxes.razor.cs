using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class ApproverKpiBoxes
    {
        [Parameter] public int Total { get; set; }
        [Parameter] public int ForApprovalCount { get; set; }
        [Parameter] public int SendBackCount { get; set; }
        [Parameter] public int ApprovedCount { get; set; }

        /// <summary>
        /// The role's pending approval workflow: PE Head = 4,
        /// HROD Head = 6, President = 7.
        /// </summary>
        [Parameter, EditorRequired] public int ForApprovalWorkFlowId { get; set; }

        [Parameter] public int SendBackWorkFlowId { get; set; } = 10;
        [Parameter] public int ApprovedWorkFlowId { get; set; } = 8;

        /// <summary>
        /// Reports null for Total or the selected box's configured workflow ID.
        /// The parent supplies counts and filters its eligible list.
        /// </summary>
        [Parameter] public EventCallback<int?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int? workFlowId) =>
            OnFilterChanged.InvokeAsync(workFlowId);
    }
}
