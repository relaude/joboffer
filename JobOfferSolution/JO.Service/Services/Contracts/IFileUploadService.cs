using Microsoft.AspNetCore.Components.Forms;

namespace JO.Service.Services.Contracts
{
    public interface IFileUploadService
    {
        Task<string> CreateFilePath(string transactionNumber, string fileName);
        string GetRootPath();
        string GetWwwRootPath();
        Task<byte[]> ReadFileAsync(IBrowserFile file);
    }
}