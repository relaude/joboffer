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
            try
            {
                candidateResponseRawData = await MSFormSyncService.SaveCandidateResponseRawData(userId);
                var invalidResponseCount = candidateResponseRawData.Count(response => response.InvalidResponse == true);
                await AlertService.Success(
                    $"MS-Forms sync completed successfully. Total raw data rows: {candidateResponseRawData.Count:N0}. Invalid responses: {invalidResponseCount:N0}.");
            }
            catch (Exception)
            {
                await AlertService.Error("Unable to sync MS-Forms responses. Please check the Excel file and database connection, then try again.");
            }
            finally
            {
                isSyncing = false;
            }
        }
    }
}
