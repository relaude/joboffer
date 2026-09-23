using JO.BlazorDemoApp.Components.Pages.Shared;
using JO.DataModel.Entity;
using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace JO.BlazorDemoApp.Components.Pages.Candidate;

public partial class CandidateFileList
{
    [Parameter] public int CandidateId { get; set; }
    [Inject] private ICandidateFileService FileService { get; set; } = default!;
    [Inject] private IAlertService AlertService { get; set; } = default!;
    [Inject] private IAccountService AccountService { get; set; } = default!;
    [Inject] private ILogger<CandidateFileList> Logger { get; set; } = default!;

    private JOModal? uploadModal;
    private DboxCandidates? candidate;
    private List<CandidateFiles> files = [];
    private List<DocumentType> documentTypes = [];
    private IBrowserFile? selectedFile;
    private int selectedTypeId;
    private int loadedCandidateId;
    private int loadVersion;
    private bool isLoading;
    private bool isSaving;
    private string? loadError;
    private readonly string inputId = $"candidate-file-{Guid.NewGuid():N}";
    private Guid fileInputKey = Guid.NewGuid();

    protected override async Task OnParametersSetAsync()
    {
        if (loadedCandidateId == CandidateId && candidate is not null) return;
        var version = ++loadVersion;
        var id = CandidateId;
        isLoading = true;
        candidate = null;
        files = [];
        loadError = null;
        uploadModal?.Close();
        try
        {
            var loadedCandidate = id > 0 ? await FileService.GetCandidateById(id) : null;
            if (version != loadVersion) return;
            if (loadedCandidate is null)
            {
                loadError = "Select a valid candidate to view documents.";
                return;
            }
            var types = await FileService.GetDocumentType();
            var documents = await FileService.GetCandidateFiles(id);
            if (version != loadVersion) return;
            candidate = loadedCandidate;
            documentTypes = types;
            files = documents;
            loadedCandidateId = id;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unable to load documents for candidate {CandidateId}", id);
            if (version == loadVersion) loadError = "Unable to load candidate documents. Please try again.";
        }
        finally { if (version == loadVersion) isLoading = false; }
    }

    private void OpenUpload()
    {
        selectedFile = null;
        selectedTypeId = 0;
        fileInputKey = Guid.NewGuid();
        uploadModal?.Show();
    }

    private async Task SelectFile(InputFileChangeEventArgs args)
    {
        selectedFile = args.File;
        if (selectedFile.Size <= 0 || selectedFile.Size > CandidateFileService.MaxFileSize)
        {
            selectedFile = null;
            await AlertService.Errors(["Select a non-empty file no larger than 3 MB."]);
        }
    }

    private async Task Upload()
    {
        if (isSaving) return;
        isSaving = true;
        var id = CandidateId;
        var version = loadVersion;
        var file = selectedFile;
        var typeId = selectedTypeId;
        try
        {
            var errors = new List<string>();
            if (candidate is null || candidate.Id != id || string.IsNullOrWhiteSpace(candidate.DboxRefNum))
                errors.Add("A valid candidate with a DBox reference number is required.");
            if (!documentTypes.Any(t => t.Id == typeId)) errors.Add("Select a document type.");
            if (file is null) errors.Add("Select a file to upload.");
            else if (file.Size <= 0 || file.Size > CandidateFileService.MaxFileSize)
                errors.Add("Select a non-empty file no larger than 3 MB.");
            if (errors.Count > 0)
            {
                await AlertService.Errors(errors);
                return;
            }
            if (!await AlertService.Confirm("Upload this candidate document?", "Upload")) return;
            if (version != loadVersion || id != CandidateId) return;
            var userId = await AccountService.GetJobOfferUserId();
            await using var stream = file!.OpenReadStream(CandidateFileService.MaxFileSize);
            var record = new CandidateFiles { CandidateId = id, TypeId = typeId, FileName = file.Name, CreatedBy = userId };
            await FileService.InsertCandidateFiles(record, stream);
            if (version == loadVersion && id == CandidateId)
            {
                files.Insert(0, record);
                uploadModal?.Close();
                selectedFile = null;
            }
            await AlertService.Success("Candidate document uploaded successfully.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unable to upload document for candidate {CandidateId}", id);
            await AlertService.Errors([ex is InvalidOperationException
                ? System.Net.WebUtility.HtmlEncode(ex.Message)
                : "Unable to upload the document. Please try again."]);
        }
        finally { isSaving = false; }
    }
}
