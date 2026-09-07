using JO.Persistence;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace JO.Service.Services
{
    public class OneDriveService : IOneDriveService
    {

        private string LocalOneDrivePath;// = @"C:\ONEDRIVE\OneDrive - United Laboratories, Inc";
        private string ExcelDestinationPath => Path.Combine(_env.WebRootPath, "excel");

        private readonly IWebHostEnvironment _env;
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IAppSettings _appSettings;
        public OneDriveService(IWebHostEnvironment env, 
            IDbContextFactory<JobOfferDbContext> dbContext,
            IAppSettings appSettings)
        {
            _env = env;
            _dbContext = dbContext;
            _appSettings = appSettings;

            LocalOneDrivePath = _appSettings.GetOneDriveLocation();
        }

        /// <summary>
        /// Copies a file from the local OneDrive root into the application's public excel folder.
        /// An existing destination file with the same name is overwritten.
        /// </summary>
        /// <param name="filename">The file name, including its extension, without a directory path.</param>
        /// <returns>The application-relative URL of the copied file.</returns>
        public async Task<string> DownloadFileAsync(string filename)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filename);

            if (Path.IsPathRooted(filename)
                || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || filename.Contains('/')
                || filename.Contains('\\')
                || filename == "."
                || filename == "..")
            {
                throw new ArgumentException("Provide a file name without a directory path.", nameof(filename));
            }

            var sourcePath = Path.Combine(LocalOneDrivePath, filename);
            var destinationPath = Path.Combine(ExcelDestinationPath, filename);

            // Open the source first so a missing or inaccessible file cannot truncate the destination.
            await using var source = new FileStream(
                sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 81920, useAsync: true);

            Directory.CreateDirectory(ExcelDestinationPath);

            await using var destination = new FileStream(
                destinationPath, FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 81920, useAsync: true);

            await source.CopyToAsync(destination);

            return $"/excel/{Uri.EscapeDataString(filename)}";
        }
    }
}
