using Microsoft.AspNetCore.Components.Forms;

namespace JO.Service.Services.Contracts
{
    public interface IJOFileService
    {
        /// <summary>
        /// Saves a file (up to 10 MB) under wwwroot/docs/{joNumber}/{fileName}.
        /// Creates the JO directory when needed and replaces an existing file after a successful upload.
        /// Returns the saved file's absolute path. JO number and file name must be single path segments.
        /// </summary>
        Task<string> SaveJobOfferFileAsync(IBrowserFile file, string joNumber, string fileName);
        /// <summary>Saves generated file contents using the same directory and size rules.</summary>
        Task<string> SaveJobOfferFileAsync(byte[] content, string joNumber, string fileName);
    }
}
