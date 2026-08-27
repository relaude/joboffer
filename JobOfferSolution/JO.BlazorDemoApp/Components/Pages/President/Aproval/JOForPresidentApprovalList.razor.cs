using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.President.Aproval
{
    public partial class JOForPresidentApprovalList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            filteredJODboxCandidates = joDboxCandidates
                .Where(jo => jo.WorkFlowId == 7 && jo.OfferRangeId == 3)
                .ToList();
        }
    }
}
