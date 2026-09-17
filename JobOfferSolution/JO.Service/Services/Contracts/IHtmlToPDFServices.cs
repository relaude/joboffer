namespace JO.Service.Services.Contracts
{
    public interface IHtmlToPDFServices
    {
        /// <summary>
        /// Renders HTML as PDF bytes. Downloads the required browser if not cached.
        /// </summary>
        /// <param name="html">The HTML document to render.</param>
        /// <returns>The generated PDF file contents.</returns>
        Task<byte[]> GeneratePdfAsync(string html);

        /// <summary>
        /// Navigates to a page on the current host and renders it using print CSS, including
        /// linked stylesheets, fonts, images, backgrounds, and CSS page sizes/margins.
        /// Current request cookies are forwarded for authorized pages. Call from an HTTP request
        /// for authenticated rendering; a Blazor circuit may not have a current HttpContext.
        /// </summary>
        /// <param name="url">A host-relative path (e.g. /ta-partner/joboffer-pdf), or a same-origin absolute URL.
        /// A leading slash resolves from the host root; include PathBase when hosted under a subpath.</param>
        /// <param name="baseUrl">Optional absolute host/base URL, required outside an HTTP request.
        /// Without a request, no authentication cookies are available. Use only a trusted application URL.</param>
        /// <param name="readySelector">Optional visible CSS selector that signals the page content is ready.
        /// For the job offer letter, use .offer-letter. This does not limit which content is printed.</param>
        /// <returns>The generated PDF file contents.</returns>
        Task<byte[]> GeneratePdfFromUrlAsync(string url, string? baseUrl = null, string? readySelector = null);
    }
}
