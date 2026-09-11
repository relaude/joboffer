using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Service.Extensions;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class DboxCandidateList
    {
        [Inject] private IDBoxAPISyncService DBoxAPISyncService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;

        private List<DboxCandidatesRawData> responses = new();
        private List<DboxCandidatesRawData> filteredResponses = new();
        private PagedResult<DboxCandidatesRawData> pagedResponses = new() { Page = 1, PageSize = 10 };
        private string candidateDBoxIdSearch = string.Empty;
        private string candidateNameSearch = string.Empty;
        private string emailSearch = string.Empty;
        private bool isLoading = true;
        private bool isImporting;
        private string? loadError;
        private Shared.JOModal? importModal;
        private IBrowserFile? importFile;
        private const long MaxImportFileSize = 10 * 1024 * 1024;

        protected override async Task OnInitializedAsync() => await LoadResponsesAsync();

        private async Task LoadResponsesAsync()
        {
            isLoading = true;
            loadError = null;
            responses.Clear();
            filteredResponses.Clear();
            ChangePage(1);
            try
            {
                responses = await DBoxAPISyncService.GetDboxCandidatesRawData();
                SearchResponses();
            }
            catch (Exception)
            {
                loadError = "Unable to load DBox candidates. Please try again.";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ChangePage(int page) =>
            pagedResponses = filteredResponses.ToPagedResult(page, pagedResponses.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedResponses = filteredResponses.ToPagedResult(1, pageSize);

        private void SearchResponses()
        {
            var id = candidateDBoxIdSearch.Trim();
            var name = candidateNameSearch.Trim();
            var email = emailSearch.Trim();
            filteredResponses = responses.Where(candidate =>
                MatchesPartial(candidate.CandidateId, id) &&
                MatchesPartial(candidate.CandidateName, name) &&
                MatchesPartial(candidate.EmailAddress, email)).ToList();
            ChangePage(1);
        }

        private void ClearSearch()
        {
            candidateDBoxIdSearch = candidateNameSearch = emailSearch = string.Empty;
            SearchResponses();
        }

        private static bool MatchesPartial(string? value, string term) =>
            term.Length == 0 || (value?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false);

        private static string DisplayValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value;

        private void OpenImportModal()
        {
            if (isLoading || isImporting)
                return;
            importFile = null;
            importModal?.Show();
        }

        private void SelectImportFile(InputFileChangeEventArgs args) => importFile = args.File;
        private void CloseImportModal() => importModal?.Close();

        private async Task ImportExcelAsync()
        {
            if (isImporting || isLoading)
                return;
            var file = importFile;
            if (file is null)
            {
                await AlertService.Error("Please select an Excel file.");
                return;
            }

            isImporting = true;
            try
            {
                var userId = await AccountService.GetJobOfferUserId();
                await using var source = file.OpenReadStream(MaxImportFileSize);
                var imported = await DBoxAPISyncService.SaveDboxCandidatesRawData(source, userId);
                CloseImportModal();
                importFile = null;
                await LoadResponsesAsync();
                await AlertService.Success($"Imported {imported.Count:N0} DBox candidates.", "Import complete");
            }
            catch (Exception)
            {
                await AlertService.Error("Unable to import DBox candidates. Use an unprotected .xlsx file no larger than 10 MB and check the database connection, then try again.", "Import failed");
            }
            finally
            {
                isImporting = false;
            }
        }
    }
}
