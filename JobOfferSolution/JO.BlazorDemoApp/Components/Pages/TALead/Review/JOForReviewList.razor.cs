using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.TALead.Review
{
    public partial class JOForReviewList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            filteredJODboxCandidates = joDboxCandidates.Where(jo => jo.WorkFlowId == 3).ToList();
        }
    }
}
