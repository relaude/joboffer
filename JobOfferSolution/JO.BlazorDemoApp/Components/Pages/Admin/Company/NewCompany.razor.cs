using JO.DataModel.Entity;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Company
{
    public partial class NewCompany
    {
        [Inject] private ICompanyService CompanyService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private ILogger<NewCompany> Logger { get; set; } = default!;

        private readonly Companies company = new();
        private readonly List<Divisions> divisions = new() { new Divisions() };
        private bool isSaving;
        private string? saveError;

        private void AddDivision()
        {
            if (!isSaving) divisions.Add(new Divisions());
        }

        private void RemoveDivision(Divisions division)
        {
            if (!isSaving && divisions.Count > 1) divisions.Remove(division);
        }

        private void Cancel()
        {
            if (!isSaving) Navigation.NavigateTo(JORoutes.Admin.Companies);
        }

        private async Task SaveCompany()
        {
            if (isSaving) return;
            isSaving = true;
            saveError = null;
            int companyId;
            try
            {
                companyId = await CompanyService.CreateCompany(company, divisions);
            }
            catch (ValidationException exception)
            {
                saveError = exception.Message;
                return;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "Unable to create company with divisions.");
                saveError = "Unable to save the company. Please try again.";
                return;
            }
            finally
            {
                isSaving = false;
            }
            Navigation.NavigateTo($"{JORoutes.Admin.CompanyDetails}/{companyId}");
        }
    }
}
