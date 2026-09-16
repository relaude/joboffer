using JO.Service.Services.Contracts;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace JO.Service.Services
{
    public class HtmlToPDFServices : IHtmlToPDFServices
    {
        private static readonly SemaphoreSlim BrowserDownloadLock = new(1, 1);

        public async Task<byte[]> GeneratePdfAsync(string html)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(html);

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

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = executablePath
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html, new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
            });

            return await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                PreferCSSPageSize = true
            });
        }
    }
}
