namespace JO.Service.Services.Contracts
{
    public interface IOneDriveService
    {
        Task<string> DownloadFileAsync(string filename);
    }
}