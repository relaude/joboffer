using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.KPIBox
{
    public partial class ReviewerKpiBoxes
    {
        [Parameter] public int Total { get; set; }
        [Parameter] public int ForReviewCount { get; set; }
        [Parameter] public int SendBackCount { get; set; }
        [Parameter] public int ReviewedCount { get; set; }

        /// <summary>
        /// Workflow IDs included in the role's For Review filter.
        /// </summary>
        [Parameter, EditorRequired] public int[] ForReviewWorkFlowIds { get; set; } = [];

        [Parameter] public int[] SendBackWorkFlowIds { get; set; } = [10];
        [Parameter] public int[] ReviewedWorkFlowIds { get; set; } = [8];

        /// <summary>
        /// Reports null for Total or the selected box's configured workflow IDs.
        /// An empty array matches no workflows.
        /// The parent supplies counts and filters its eligible list.
        /// </summary>
        [Parameter] public EventCallback<int[]?> OnFilterChanged { get; set; }

        private Task FilterByWorkFlow(int[]? workFlowIds) =>
            OnFilterChanged.InvokeAsync(workFlowIds);
    }
}
