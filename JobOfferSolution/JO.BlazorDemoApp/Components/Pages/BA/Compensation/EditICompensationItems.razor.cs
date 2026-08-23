using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Compensation
{
    public partial class EditICompensationItems
    {
        [Parameter] public int compensationId { get; set; }

        [Inject] private ICompensationService CompensationService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private VwCompanyCompensation compensation = new();
        private List<VwCompanyCompensationItems> compensationItems = new();
        private bool isSaving;
        private int userId;
        private string DetailsRoute => $"{JORoutes.BA.Compensations}/{compensationId}";

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            var compensations = await CompensationService.GetVwCompanyCompensation();
            compensation = compensations.FirstOrDefault(jo => jo.Id == compensationId) ?? new();
            compensationItems = await CompensationService.GetVwCompanyCompensationItems(compensationId);
        }

        private async Task SaveCompensationItems()
        {
            if (isSaving || !await AlertService.Confirm("Save changes to these compensation items?", "Save"))
                return;

            isSaving = true;

            try
            {
                await CompensationService.UpdateCompensationItems(compensationItems, userId);
                await AlertService.Success("Compensation items successfully updated.");
                Navigation.NavigateTo(DetailsRoute);
            }
            finally
            {
                isSaving = false;
            }
        }
    }
}
