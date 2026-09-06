using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.KPIBox
{
    public partial class JobOfferKpiBoxes
    {
        [Parameter] public int Total { get; set; }
        [Parameter] public int ForReviewCount { get; set; }
        [Parameter] public int ReviewedCount { get; set; }
        [Parameter] public int SendBackCount { get; set; }
        [Parameter] public int ForApprovalCount { get; set; }
        [Parameter] public int ForDiscussionCount { get; set; }
        [Parameter] public int ApprovedCount { get; set; }
        [Parameter] public int AcceptedCount { get; set; }
        [Parameter] public int ForNegotiationCount { get; set; }
        [Parameter] public int DeclinedCount { get; set; }

        [Parameter] public int[] ForReviewWorkFlowIds { get; set; } = [3];
        [Parameter] public int[] ReviewedWorkFlowIds { get; set; } = [4];
        [Parameter] public int[] SendBackWorkFlowIds { get; set; } = [10];
        [Parameter, EditorRequired] public int[] ForApprovalWorkFlowIds { get; set; } = [];
        [Parameter] public int[] ForDiscussionWorkFlowIds { get; set; } = [8];
        [Parameter] public int[] ApprovedWorkFlowIds { get; set; } = [0];
        [Parameter] public int[] AcceptedWorkFlowIds { get; set; } = [9];
        [Parameter] public int[] ForNegotiationWorkFlowIds { get; set; } = [11];
        [Parameter] public int[] DeclinedWorkFlowIds { get; set; } = [12];

        private (string Label, int Count, int[]? WorkFlowIds, bool IsTotal)[] KpiBoxes =>
        [
            ("Total", Total, null, true),
            ("For Review", ForReviewCount, ForReviewWorkFlowIds, false),
            ("Reviewed", ReviewedCount, ReviewedWorkFlowIds, false),
            ("For Approval", ForApprovalCount, ForApprovalWorkFlowIds, false),
            ("Approved", ApprovedCount, ApprovedWorkFlowIds, false),
            ("Accepted", AcceptedCount, AcceptedWorkFlowIds, false),
            ("Declined", DeclinedCount, DeclinedWorkFlowIds, false),
            ("For Negotiation", ForNegotiationCount, ForNegotiationWorkFlowIds, false),
            ("Send Back", SendBackCount, SendBackWorkFlowIds, false),
            ("For Discussion", ForDiscussionCount, ForDiscussionWorkFlowIds, false)
        ];

        /// <summary>
        /// Reports null for Total or the selected box's configured workflow IDs.
        /// An empty array matches no workflows. The parent supplies counts and filters its eligible list.
        /// </summary>
        [Parameter] public EventCallback<int[]?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int[]? workFlowIds) =>
            OnFilterChanged.InvokeAsync(workFlowIds);
    }
}
