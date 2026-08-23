using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Compensation
{
    public partial class CompensationDetails
    {
        [Parameter] public int compensationId { get; set; }

        [Inject] private ICompensationService CompensationService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        private VwCompanyCompensation compensation = new();
        private List<VwCompanyCompensationItems> compensationItems = new();

        protected override async Task OnParametersSetAsync()
        {
            var compensations = await CompensationService.GetVwCompanyCompensation();
            compensation = compensations.FirstOrDefault(jo => jo.Id == compensationId) ?? new();
            compensationItems = await CompensationService.GetVwCompanyCompensationItems(compensationId);
        }
    }
}
