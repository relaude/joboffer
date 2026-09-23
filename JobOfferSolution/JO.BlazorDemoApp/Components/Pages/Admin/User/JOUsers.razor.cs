using JO.DataModel.DTOs;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.User
{
    public partial class JOUsers
    {
        [Inject] private IManageUsersService ManageUsersService { get; set; } = default!;

        private PagedResult<VwJobOfferUsers> pagedUsers = new() { Page = 1, PageSize = 10 };
        private string nameSearch = string.Empty;
        private string emailSearch = string.Empty;
        private string statusFilter = string.Empty;
        private string appliedName = string.Empty;
        private string appliedEmail = string.Empty;
        private bool? appliedStatus;
        private bool isLoading = true;
        private string? loadError;

        protected override Task OnInitializedAsync() => LoadUsersAsync();

        private async Task LoadUsersAsync()
        {
            isLoading = true;
            loadError = null;
            try
            {
                pagedUsers = await ManageUsersService.GetPagedJobOfferUsers(
                    appliedName, appliedEmail, appliedStatus, pagedUsers.Page, pagedUsers.PageSize);
            }
            catch (Exception)
            {
                pagedUsers.Data.Clear();
                pagedUsers.TotalCount = 0;
                loadError = "Unable to load users. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private async Task SearchUsers()
        {
            if (isLoading) return;
            appliedName = nameSearch.Trim();
            appliedEmail = emailSearch.Trim();
            appliedStatus = statusFilter switch
            {
                "Active" => true,
                "InActive" => false,
                _ => null
            };
            await ChangePage(1);
        }

        private async Task ClearSearch()
        {
            nameSearch = emailSearch = string.Empty;
            statusFilter = string.Empty;
            await SearchUsers();
        }

        private async Task ChangePage(int page)
        {
            pagedUsers.Page = page;
            await LoadUsersAsync();
        }

        private async Task ChangePageSize(int pageSize)
        {
            pagedUsers.PageSize = pageSize;
            await ChangePage(1);
        }
    }
}
