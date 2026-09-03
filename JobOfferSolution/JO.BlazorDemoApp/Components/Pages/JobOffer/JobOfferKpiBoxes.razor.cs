using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class JobOfferKpiBoxes
    {
        // The parent maps this value to workflows 4, 5, 6, and 7.
        public const int ForApprovalFilter = -1;
        public const int ForNegotiationWorkFlowId = 11;
        public const int DeclinedWorkFlowId = 12;

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

        private (string Label, int Count, int? WorkFlowId, bool IsTotal)[] KpiBoxes =>
        [
            ("Total", Total, null, true),
            ("For Review", ForReviewCount, 3, false),
            ("Reviewed", ReviewedCount, 4, false),
            ("For Approval", ForApprovalCount, ForApprovalFilter, false),
            ("Approved", ApprovedCount, 8, false),
            ("Accepted", AcceptedCount, 9, false),
            ("Declined", DeclinedCount, DeclinedWorkFlowId, false),
            ("For Negotiation", ForNegotiationCount, ForNegotiationWorkFlowId, false),
            ("Send Back", SendBackCount, 10, false),
            ("For Discussion", ForDiscussionCount, 8, false)
        ];

        /// <summary>
        /// Reports null for Total, 3 for For Review, 4 for Reviewed,
        /// 10 for Send Back, -1 for For Approval, 8 for For Discussion and Approved,
        /// 9 for Accepted, 11 for For Negotiation, and 12 for Declined.
        /// </summary>
        [Parameter] public EventCallback<int?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int? workFlowId) =>
            OnFilterChanged.InvokeAsync(workFlowId);
    }
}
