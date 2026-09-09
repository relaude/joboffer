using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace JO.BlazorDemoApp.Components.Pages.Admin.Candidate
{
    public partial class CandidateResponses
    {
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private IOneDriveService OneDriveService { get; set; } = default!;
        [Inject] private IMSFormSyncService MSFormSyncService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;

        private List<DataModel.Entity.CandidateResponses> candidateResponses = new();
        private List<CandidateResponseRawData> candidateResponseRawData = new();
        private bool isImporting;
        private bool isSyncing;
        private int userId;

        protected override async Task OnInitializedAsync()
        {
            candidateResponses = await CandidateService.GetCandidateResponses();
            userId = await AccountService.GetJobOfferUserId();
        }

        private async Task ImportFromExcelAsync()
        {
            if (isImporting || isSyncing)
                return;

            isImporting = true;
            try
            {
                await OneDriveService.DownloadFileAsync("CandidateResponseSample.xlsx");
                await AlertService.Confirm("CandidateResponseSample.xlsx downloaded successfully.", "OK", "Close");
            }
            catch (Exception)
            {
                await AlertService.Error("Unable to download CandidateResponseSample.xlsx. Check that the file exists in the local OneDrive folder and the application has permission to read it and write to the excel folder.");
            }
            finally
            {
                isImporting = false;
            }
        }

        private async Task SyncMSFormsAsync()
        {
            if (isSyncing || isImporting)
                return;

            isSyncing = true;
            var downloadCompleted = false;
            try
            {
                await OneDriveService.DownloadFileAsync("CandidateResponseSample.xlsx");
                downloadCompleted = true;

                candidateResponseRawData = await MSFormSyncService.SaveCandidateResponseRawData(userId);
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
    }
}
