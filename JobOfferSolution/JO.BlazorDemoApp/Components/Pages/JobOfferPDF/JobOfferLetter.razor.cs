using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace JO.BlazorDemoApp.Components.Pages.JobOfferPDF
{
    public partial class JobOfferLetter
    {
        public const string PdfDownloadRoute = JORoutes.TAPartner.JobOfferPdf + "/download";

        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IJSRuntime JS { get; set; } = default!;
        [Inject] private ILogger<JobOfferLetter> Logger { get; set; } = default!;

        private bool isDownloadingPdf;
        private string? pdfDownloadError;

        private string PdfDownloadUrl => Navigation.ToAbsoluteUri(PdfDownloadRoute.TrimStart('/')).AbsoluteUri;

        private async Task GenerateAndDownloadPdfAsync()
        {
            if (isDownloadingPdf)
                return;

            isDownloadingPdf = true;
            pdfDownloadError = null;

            try
            {
                await using var module = await JS.InvokeAsync<IJSObjectReference>("import",
                    Navigation.ToAbsoluteUri("Components/Pages/JobOfferPDF/JobOfferLetter.razor.js").AbsoluteUri);
                // Allow conversion more time than the default JS interop timeout.
                using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(4));
                await module.InvokeVoidAsync("downloadPdf", timeout.Token, PdfDownloadUrl);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to download the job offer letter PDF.");
                pdfDownloadError = "The PDF could not be downloaded. Please try again. If your session has expired, sign in again.";
            }
            finally
            {
                isDownloadingPdf = false;
            }
        }

        // Endpoint injection keeps rendering inside a real authenticated HTTP request,
        // so GeneratePdfFromUrlAsync can forward the caller's cookies to the preview URL.
        public static async Task<IResult> DownloadPdfAsync(
            HttpContext context,
            [FromServices] IHtmlToPDFServices htmlToPDFServices,
            [FromServices] ILogger<JobOfferLetter> logger)
        {
            context.Response.Headers.CacheControl = "no-store";

            try
            {
                // Render the preview, never the download endpoint (which would recurse).
                var previewUrl = context.Request.PathBase.Add(new PathString(JORoutes.TAPartner.JobOfferPdf));
                var pdfBytes = await htmlToPDFServices.GeneratePdfFromUrlAsync(
                    previewUrl.Value!, readySelector: ".offer-letter");

                return Results.File(pdfBytes, "application/pdf", "Job-Offer-Letter.pdf");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to generate the job offer letter PDF from its preview URL.");
                return Results.Problem(
                    title: "Unable to generate the job offer letter PDF",
                    detail: "Return to the preview and try again. If the issue continues, contact support.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
