using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Compensation
{
    public partial class CompensationList
    {
        [Inject]
        private ICompensationService CompensationService { get; set; } = default!;

        private List<VwCompanyCompensation> compensationList = new();

        protected override async Task OnInitializedAsync()
        {
            compensationList = await CompensationService.GetVwCompanyCompensation();
        }
    }
}
