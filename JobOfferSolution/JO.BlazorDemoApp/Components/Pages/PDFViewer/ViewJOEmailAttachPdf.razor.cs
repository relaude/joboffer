using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.PDFViewer;

public partial class ViewJOEmailAttachPdf
{
    [Inject] private IJOLetterService JOLetterService { get; set; } = default!;
    [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
    [Inject] private ILogger<ViewJOEmailAttachPdf> Logger { get; set; } = default!;

    [Parameter] public int Id { get; set; }

    private bool isLoading = true;
    private string? fileName;
    private string? pdfContent;
    private string? loadError;
    private int loadVersion;

    protected override async Task OnParametersSetAsync()
    {
        var version = ++loadVersion;
        var attachmentId = Id;
        isLoading = true;
        fileName = null;
        pdfContent = null;
        loadError = null;

        try
        {
            var attachment = await JOLetterService.GetJOHasEmailAttachById(attachmentId);
            if (version != loadVersion)
                return;

            if (attachment is null)
            {
                loadError = "This attachment could not be found.";
                return;
            }

            var relativePath = attachment.RelativePath?.Replace('\\', '/');
            if (string.IsNullOrWhiteSpace(relativePath) || Path.IsPathRooted(relativePath))
                throw new InvalidOperationException("The attachment path is invalid.");

            var docsRoot = Path.GetFullPath(Path.Combine(Environment.WebRootPath, "docs")) + Path.DirectorySeparatorChar;
            var path = Path.GetFullPath(Path.Combine(Environment.WebRootPath, relativePath));
            var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            if (!path.StartsWith(docsRoot, comparison))
                throw new InvalidOperationException("The attachment must be stored in the docs directory.");

            if (!string.Equals(Path.GetExtension(path), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                loadError = "This attachment is not a PDF document.";
                return;
            }

            var bytes = await File.ReadAllBytesAsync(path);
            if (version != loadVersion)
                return;

            fileName = string.IsNullOrWhiteSpace(attachment.FileName) ? Path.GetFileName(path) : attachment.FileName;
            pdfContent = Convert.ToBase64String(bytes);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load PDF attachment {AttachmentId}.", attachmentId);
            if (version == loadVersion)
                loadError = "Unable to load this PDF attachment. The file may be missing or unavailable.";
        }
        finally
        {
            if (version == loadVersion)
                isLoading = false;
        }
    }
}
