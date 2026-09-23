using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Company
{
    public partial class EditCompany
    {
        [Parameter] public int companyId { get; set; }
        [Inject] private ICompanyService CompanyService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private ILogger<EditCompany> Logger { get; set; } = default!;

        private VwCompanies? company;
        private List<VwDivisions> divisions = new();
        private bool isLoading = true;
        private bool isSaving;
        private string? loadError;
        private string? saveError;
        private int loadVersion;

        protected override Task OnParametersSetAsync() => LoadCompany();

        private async Task LoadCompany()
        {
            var version = ++loadVersion;
            var id = companyId;
            isLoading = true;
            company = null;
            divisions.Clear();
            loadError = saveError = null;
            try
            {
                var loadedCompany = await CompanyService.GetVwCompany(id);
                if (version != loadVersion) return;
                if (loadedCompany.Id == 0)
                {
                    loadError = "Company not found. Return to the company list.";
                    return;
                }
                var loadedDivisions = await CompanyService.GetVwDivisions(id);
                if (version != loadVersion) return;
                company = loadedCompany;
                divisions = loadedDivisions;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to load company {CompanyId} for editing.", id);
                if (version == loadVersion) loadError = "Unable to load the company. Please try again.";
            }
            finally
            {
                if (version == loadVersion) isLoading = false;
            }
        }

        private void Cancel()
        {
            if (!isSaving) Navigation.NavigateTo(JORoutes.Admin.Companies);
        }

        private async Task SaveCompany()
        {
            if (isSaving || isLoading || company is null) return;
            isSaving = true;
            saveError = null;
            var id = company.Id;
            var version = loadVersion;
            try
            {
                await CompanyService.UpdateCompanyNames(id, company.CompanyName ?? string.Empty,
                    divisions.ToDictionary(division => division.Id, division => division.DivisionName ?? string.Empty));
            }
            catch (ValidationException exception)
            {
                if (version == loadVersion) saveError = exception.Message;
                return;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to update names for company {CompanyId}.", id);
                if (version == loadVersion) saveError = "Unable to save changes. Please try again.";
                return;
            }
            finally
            {
                isSaving = false;
            }
            if (version == loadVersion) Navigation.NavigateTo($"{JORoutes.Admin.CompanyDetails}/{id}");
        }
    }
}
