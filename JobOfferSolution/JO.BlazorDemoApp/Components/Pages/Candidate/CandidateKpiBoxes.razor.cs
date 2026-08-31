using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Candidate
{
    public partial class CandidateKpiBoxes
    {
        public const int WithResponseFilter = -1;
        public const int WithoutResponseFilter = -2;

        [Parameter] public int Total { get; set; }
        [Parameter] public int WithResponseCount { get; set; }
        [Parameter] public int NoResponseCount { get; set; }
        [Parameter] public int CreatedCount { get; set; }
        [Parameter] public int DraftCount { get; set; }
        [Parameter] public int ForCreationCount { get; set; }

        /// <summary>
        /// Reports null for Total, -1 for With Response, -2 for No Response,
        /// 3 for Created, 2 for Draft, and 1 for For Creation.
        /// The parent supplies counts and applies filters to its eligible candidates.
        /// </summary>
        [Parameter] public EventCallback<int?> OnFilterChanged { get; set; }

        private Task FilterCandidates(int? filter) =>
            OnFilterChanged.InvokeAsync(filter);
    }
}
