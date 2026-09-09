using JO.DataModel.DTOs;
using JO.Service.Extensions;
using JO.DataModel.Entity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class ResponseRawData
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private IOneDriveService OneDriveService { get; set; } = default!;
        [Inject] private IMSFormSyncService MSFormSyncService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;

        private List<CandidateResponseRawData> responses = new();
        private List<CandidateResponseRawData> filteredResponses = new();
        private string candidateDBoxIdSearch = string.Empty;
        private string candidateNameSearch = string.Empty;
        private string emailSearch = string.Empty;
        private string validationStatusSearch = string.Empty;
        private bool isLoading = true;
        private bool isSyncing;
        private string lastSyncDisplay = "Loading...";
        private string? loadError;
        private PagedResult<CandidateResponseRawData> pagedResponses = new() { Page = 1, PageSize = 10 };

        private void ChangePage(int page) =>
            pagedResponses = filteredResponses.ToPagedResult(page, pagedResponses.PageSize);

        private void ChangePageSize(int pageSize) =>
            pagedResponses = filteredResponses.ToPagedResult(1, pageSize);

        private void SearchResponses()
        {
            var dboxId = candidateDBoxIdSearch.Trim();
            var name = candidateNameSearch.Trim();
            var email = emailSearch.Trim();

            filteredResponses = responses.Where(response =>
                MatchesPartial(response.CandidateDBoxID, dboxId) &&
                MatchesPartial(GetCandidateName(response), name) &&
                MatchesPartial(response.EmailAddress, email) &&
                (validationStatusSearch switch
                {
                    "needs-review" => response.InvalidResponse == true,
                    "valid" => response.InvalidResponse == false,
                    "not-validated" => response.InvalidResponse is null,
                    _ => true
                })).ToList();

            ChangePage(1);
        }

        private void ClearSearch()
        {
            candidateDBoxIdSearch = candidateNameSearch = emailSearch = validationStatusSearch = string.Empty;
            SearchResponses();
        }

        // In-memory equivalent of a case-insensitive SQL LIKE '%term%' search.
        private static bool MatchesPartial(string? value, string term) =>
            term.Length == 0 || (value?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false);

        private static string? GetCandidateName(CandidateResponseRawData response) =>
            string.IsNullOrWhiteSpace(response.CandidateFullName) ? response.RespondentName : response.CandidateFullName;

        protected override async Task OnInitializedAsync()
        {
            await LoadResponsesAsync();
        }

        private async Task LoadResponsesAsync()
        {
            isLoading = true;
            lastSyncDisplay = "Loading...";
            loadError = null;
            responses.Clear();
            filteredResponses.Clear();
            ChangePage(1);
            try
            {
                responses = await CandidateService.GetCandidateResponsesRawData();
                SearchResponses();
            }
            catch (Exception)
            {
                loadError = "Unable to load candidate responses. Please try again.";
            }
            finally
            {
                await LoadLastSyncAsync();
                isLoading = false;
            }
        }

        private async Task LoadLastSyncAsync()
        {
            try
            {
                var lastSync = await MSFormSyncService.GetLatestDateTimeMSFormAsync();
                lastSyncDisplay = lastSync?.ToString("dd MMM yyyy, HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                    ?? "Not synced yet";
            }
            catch (Exception)
            {
                lastSyncDisplay = "Unavailable";
            }
        }

        private async Task SyncMSFormsAsync()
        {
            if (isSyncing || isLoading)
                return;

            isSyncing = true;
            var downloadCompleted = false;
            try
            {
                int userId;
                try
                {
                    userId = await AccountService.GetJobOfferUserId();
                }
                catch (Exception)
                {
                    await AlertService.Error("Unable to identify the current user. Please sign in again and retry MS-Forms Sync.", "Sync failed");
                    return;
                }

                await OneDriveService.DownloadFileAsync("CandidateResponseSample.xlsx");
                downloadCompleted = true;

                var candidateResponseRawData = await MSFormSyncService.SaveCandidateResponseRawData(userId);
                await LoadResponsesAsync();
                var invalidResponseCount = candidateResponseRawData.Count(response => response.InvalidResponse == true);
                if (candidateResponseRawData.Count == 0)
                {
                    await AlertService.Success(
                        "The workbook was downloaded, but no responses were found to save. Check that the source workbook contains response rows.",
                        "No responses to sync");
                }
                else
                {
                    var message = $"The workbook was downloaded and {candidateResponseRawData.Count:N0} responses were saved.";
                    if (invalidResponseCount > 0)
                    {
                        message += $" {invalidResponseCount:N0} saved responses have validation issues. Review their errors and correct the source data.";
                    }

                    await AlertService.Success(message,
                        invalidResponseCount > 0 ? "Sync completed with validation issues" : "Sync complete");
                }
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException ex)
            {
                await AlertService.Error($"{ex.Message} Correct the source workbook, then select MS-Forms Sync to try again.",
                    "Workbook needs attention");
            }
            catch (Exception)
            {
                await AlertService.Error(
                    downloadCompleted
                        ? "The workbook was downloaded, but the responses could not be synced. Check the workbook format and database connection, then try again."
                        : "Could not download CandidateResponseSample.xlsx from OneDrive. No responses were saved. Check that the file is available in the configured OneDrive folder and the application can access it, then try again.",
                    downloadCompleted ? "Sync failed" : "Download failed");
            }
            finally
            {
                isSyncing = false;
            }
        }

        private static string DisplayValue(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value;
    }
}
