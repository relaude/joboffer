using JO.Service.Constants;
using JO.DataModel.Entity;
using JO.DataModel.View;
using System.Globalization;
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
        [Inject] private IJOLetterService JOLetterService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }
        [Parameter] public decimal proposedSalary { get; set; }

        private JobOffers jobOffer = new();
        private VwDboxCandidates candidate = new();
        private List<JOItemLetter> joItemLetters = new();
        private string letterBody = string.Empty;
        private bool isLoadingLetter = true;
        private string? letterLoadError;

        protected override async Task OnParametersSetAsync()
        {
            isLoadingLetter = true;
            letterBody = string.Empty;
            letterLoadError = null;
            try
            {
                jobOffer = await JOLetterService.GetJobOffer(jobOfferId);
                if (jobOffer?.CandidateId is not int candidateId)
                {
                    letterLoadError = "The job offer or candidate could not be found.";
                    return;
                }

                candidate = await JOLetterService.GetVwDboxCandidate(candidateId);
                if (candidate is null)
                {
                    letterLoadError = "The candidate could not be found.";
                    return;
                }

                await LoadLetterBody(proposedSalary);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load the letter for job offer {JobOfferId}.", jobOfferId);
                letterLoadError = "Unable to load the job offer letter. Please try again.";
            }
            finally
            {
                isLoadingLetter = false;
            }
        }

        private async Task LoadLetterBody(decimal proposedSalary)
        {
            joItemLetters = await JOLetterService.GetJOItemLetter(jobOffer.CmpnyCmpnstnId.GetValueOrDefault());
            JOLetterService.UpdateItemLetterPlaceHolder(joItemLetters, candidate, proposedSalary);
            letterBody = string.Join("", joItemLetters
                .Where(item => !string.IsNullOrWhiteSpace(item.MessageBody))
                .Select(item => item.MessageBody));
        }

        private bool isDownloadingPdf;
        private string? pdfDownloadError;

        private static string LetterQuery(int id, decimal salary) =>
            $"?jobOfferId={id}&proposedSalary={salary.ToString(CultureInfo.InvariantCulture)}";

        private string PdfDownloadUrl => Navigation.ToAbsoluteUri(PdfDownloadRoute.TrimStart('/') + LetterQuery(jobOfferId, proposedSalary)).AbsoluteUri;

        private async Task GenerateAndDownloadPdfAsync()
        {
            if (isDownloadingPdf || isLoadingLetter || letterLoadError is not null || string.IsNullOrWhiteSpace(letterBody))
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
            [FromQuery] int jobOfferId,
            [FromQuery] decimal proposedSalary,
            [FromServices] IHtmlToPDFServices htmlToPDFServices,
            [FromServices] ILogger<JobOfferLetter> logger)
        {
            context.Response.Headers.CacheControl = "no-store";

            try
            {
                // Render the preview, never the download endpoint (which would recurse).
                var previewPath = $"{JORoutes.TAPartner.JobOfferPdf}/{jobOfferId.ToString(CultureInfo.InvariantCulture)}/{proposedSalary.ToString(CultureInfo.InvariantCulture)}";
                var previewUrl = context.Request.PathBase.Add(new PathString(previewPath));
                var pdfBytes = await htmlToPDFServices.GeneratePdfFromUrlAsync(
                    previewUrl.Value!, readySelector: ".offer-letter[data-ready='true']");

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
