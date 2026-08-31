using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class JobOfferKpiBoxes
    {
        // The parent maps this value to workflows 4, 5, 6, and 7.
        public const int ForApprovalFilter = -1;

        [Parameter] public int Total { get; set; }
        [Parameter] public int ForReviewCount { get; set; }
        [Parameter] public int ReviewedCount { get; set; }
        [Parameter] public int SendBackCount { get; set; }
        [Parameter] public int ForApprovalCount { get; set; }
        [Parameter] public int ApprovedCount { get; set; }
        [Parameter] public int AcceptedCount { get; set; }

        /// <summary>
        /// Reports null for Total, 3 for For Review, 4 for Reviewed,
        /// 10 for Send Back, -1 for For Approval, 8 for Approved, and 9 for Accepted.
        /// </summary>
        [Parameter] public EventCallback<int?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int? workFlowId) =>
            OnFilterChanged.InvokeAsync(workFlowId);
    }
}
