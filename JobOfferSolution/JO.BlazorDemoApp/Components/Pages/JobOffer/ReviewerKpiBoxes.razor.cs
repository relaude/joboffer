using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class ReviewerKpiBoxes
    {
        [Parameter] public int Total { get; set; }
        [Parameter] public int ForReviewCount { get; set; }
        [Parameter] public int SendBackCount { get; set; }
        [Parameter] public int ReviewedCount { get; set; }

        /// <summary>
        /// Reports null for Total, 3 for For Review, 10 for Send Back,
        /// and 4 for Reviewed. The parent supplies counts and filters its list.
        /// </summary>
        [Parameter] public EventCallback<int?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int? workFlowId) =>
            OnFilterChanged.InvokeAsync(workFlowId);
    }
}
