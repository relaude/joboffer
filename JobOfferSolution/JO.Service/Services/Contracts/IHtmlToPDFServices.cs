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
    }
}
