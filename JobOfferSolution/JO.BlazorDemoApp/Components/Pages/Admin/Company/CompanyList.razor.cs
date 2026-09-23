using JO.DataModel.DTOs;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Company
{
    public partial class CompanyList
    {
        [Inject] private ICompanyService CompanyService { get; set; } = default!;

        private PagedResult<VwCompanies> pagedCompanies = new() { Page = 1, PageSize = 10 };
        private string companyNameSearch = string.Empty;
        private string companyCodeSearch = string.Empty;
        private string appliedName = string.Empty;
        private string appliedCode = string.Empty;
        private bool isLoading = true;
        private string? loadError;

        protected override async Task OnInitializedAsync() => await LoadCompaniesAsync();

        private async Task LoadCompaniesAsync()
        {
            isLoading = true;
            loadError = null;
            try
            {
                pagedCompanies = await CompanyService.GetPagedCompanies(
                    appliedName, appliedCode, pagedCompanies.Page, pagedCompanies.PageSize);
            }
            catch (Exception)
            {
                pagedCompanies.Data.Clear();
                pagedCompanies.TotalCount = 0;
                loadError = "Unable to load companies. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SearchCompanies()
        {
            appliedName = companyNameSearch.Trim();
            appliedCode = companyCodeSearch.Trim();
            await ChangePage(1);
        }

        private async Task ClearSearch()
        {
            companyNameSearch = companyCodeSearch = string.Empty;
            await SearchCompanies();
        }

        private async Task ChangePage(int page)
        {
            pagedCompanies.Page = page;
            await LoadCompaniesAsync();
        }

        private async Task ChangePageSize(int pageSize)
        {
            pagedCompanies.PageSize = pageSize;
            await ChangePage(1);
        }
    }
}
