using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.DHL1.Approval.Standard
{
    public partial class JOForDHL1ApprovalList
    {
        [Inject] private IJODetailsService JODetailsService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private List<VwJODboxCandidates> joDboxCandidates = new();
        private List<VwJODboxCandidates> filteredJODboxCandidates = new();

        protected override async Task OnInitializedAsync()
        {
            joDboxCandidates = await JODetailsService.GetVwJODboxCandidates();
            filteredJODboxCandidates = joDboxCandidates
                .Where(jo => jo.WorkFlowId == 3 && jo.OfferRangeId == 1)
                .ToList();
        }
    }
}
