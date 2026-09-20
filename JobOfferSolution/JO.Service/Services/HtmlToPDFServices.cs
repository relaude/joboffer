using JO.Service.Enum;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Http;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace JO.Service.Services
{
    public class HtmlToPDFServices : IHtmlToPDFServices
    {
        private static readonly SemaphoreSlim BrowserDownloadLock = new(1, 1);
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HtmlToPDFServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<byte[]> GeneratePdfAsync(string html)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(html);

            await using var browser = await LaunchBrowserAsync();
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
            });

            return await page.PdfDataAsync(CreatePdfOptions());
        }

        public async Task<byte[]> GeneratePdfFromUrlAsync(string url, string? baseUrl = null,
            string? readySelector = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url);

            var request = _httpContextAccessor.HttpContext?.Request;
            var requestOrigin = request is null ? null : new Uri($"{request.Scheme}://{request.Host}/");
            var hostUri = baseUrl is null ? requestOrigin : new Uri(baseUrl, UriKind.Absolute);
            if (hostUri is null)
                throw new InvalidOperationException(
                    "A current HTTP request or an explicit baseUrl is required to resolve the PDF host.");

            if (hostUri.Scheme != Uri.UriSchemeHttp && hostUri.Scheme != Uri.UriSchemeHttps)
                throw new ArgumentException("The PDF host must use HTTP or HTTPS.", nameof(baseUrl));

            if (requestOrigin is not null && !IsSameOrigin(requestOrigin, hostUri))
                throw new ArgumentException("The PDF host must match the current request host.", nameof(baseUrl));

            var targetUri = new Uri(hostUri, url);
            if (!IsSameOrigin(hostUri, targetUri) || !string.IsNullOrEmpty(targetUri.UserInfo))
                throw new ArgumentException("The PDF URL must belong to the current host.", nameof(url));

            await using var browser = await LaunchBrowserAsync();
            await using var page = await browser.NewPageAsync();
            page.DefaultTimeout = 30_000;
            page.DefaultNavigationTimeout = 30_000;

            // Cookies are scoped to this host, never added as headers to external CSS/image requests.
            if (request is not null && request.Cookies.Count > 0)
            {
                await page.SetCookieAsync(request.Cookies.Select(cookie => new CookieParam
                {
                    Name = cookie.Key,
                    Value = cookie.Value,
                    Url = requestOrigin!.AbsoluteUri,
                    Path = "/",
                    HttpOnly = true,
                    Secure = request.IsHttps
                }).ToArray());
            }

            await page.EmulateMediaTypeAsync(MediaType.Print);
            var response = await page.GoToAsync(targetUri.AbsoluteUri, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.Load, WaitUntilNavigation.Networkidle2 },
                Timeout = 30_000
            });

            if (response is null || !response.Ok)
                throw new InvalidOperationException($"The PDF page returned HTTP status {response?.Status}.");

            // An authorization redirect may return HTTP 200 for a login page.
            var loadedUri = new Uri(page.Url);
            if (!IsSameOrigin(targetUri, loadedUri)
                || loadedUri.AbsolutePath.TrimEnd('/') != targetUri.AbsolutePath.TrimEnd('/')
                || loadedUri.Query != targetUri.Query)
                throw new InvalidOperationException(
                    "The PDF page redirected. Ensure the caller is signed in and authorized for the requested URL.");

            if (!string.IsNullOrWhiteSpace(readySelector))
                await page.WaitForSelectorAsync(readySelector, new WaitForSelectorOptions { Visible = true });

            // Wait for print fonts, lazy images, and their decoded pixels, with a bounded timeout.
            await page.EvaluateFunctionAsync("""
                async () => {
                    let timeout;
                    try {
                        await Promise.race([
                            (async () => {
                                await document.fonts.ready;
                                await Promise.all(Array.from(document.images, async image => {
                                    image.loading = 'eager';
                                    await image.decode();
                                }));
                            })(),
                            new Promise((_, reject) => {
                                timeout = setTimeout(() => reject(new Error('PDF assets did not load in time.')), 30000);
                            })
                        ]);
                    } finally {
                        clearTimeout(timeout);
                    }
                }
                """);

            return await page.PdfDataAsync(CreatePdfOptions());
        }

        private static bool IsSameOrigin(Uri first, Uri second) =>
            first.Scheme == second.Scheme && first.Host == second.Host && first.Port == second.Port;

        private static PdfOptions CreatePdfOptions() => new()
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            PreferCSSPageSize = true
        };

        private static async Task<IBrowser> LaunchBrowserAsync()
        {

            string executablePath;
            // Prevent concurrent requests from extracting the same browser download.
            await BrowserDownloadLock.WaitAsync();
            try
            {
                var browserFetcher = new BrowserFetcher();
                var installedBrowser = await browserFetcher.DownloadAsync();
                executablePath = installedBrowser.GetExecutablePath();
            }
            finally
            {
                BrowserDownloadLock.Release();
            }

            return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = executablePath
            });
        }
    }
}
