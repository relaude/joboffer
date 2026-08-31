using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Candidate
{
    public partial class CandidateTableList
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Parameter, EditorRequired]
        public IReadOnlyList<VwDboxCandidates> Candidates { get; set; } = [];

        /// <summary>
        /// Use JOUserRole.TA or JOUserRole.TALead to select the corresponding
        /// CandidateService link resolver when Url is not supplied.
        /// </summary>
        [Parameter, EditorRequired]
        public string RoleId { get; set; } = JOUserRole.TA;

        /// <summary>
        /// Optional route override. When supplied, the candidate ID is appended
        /// to this URL instead of resolving the link through CandidateService.
        /// </summary>
        [Parameter]
        public string? Url { get; set; }

        [Parameter]
        public string Title { get; set; } = "Candidates";

        private async Task OpenCandidate(VwDboxCandidates candidate)
        {
            var candidateLink = !string.IsNullOrWhiteSpace(Url)
                ? $"{Url.TrimEnd('/')}/{candidate.Id}"
                : RoleId == JOUserRole.TALead
                    ? await CandidateService.GetTALeadCandidateLink(candidate)
                    : await CandidateService.GetCandidateLink(candidate);

            Navigation.NavigateTo(candidateLink);
        }
    }
}
