using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace JO.Service.Services
{
    public class JOFileService : IJOFileService
    {
        private const long MaxFileSize = 10 * 1024 * 1024;
        private readonly IWebHostEnvironment _environment;

        public JOFileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveJobOfferFileAsync(IBrowserFile file, string joNumber, string fileName)
        {
            ArgumentNullException.ThrowIfNull(file);
            await using var source = file.OpenReadStream(MaxFileSize);
            return await SaveJobOfferFileAsync(source, joNumber, fileName);
        }

        public async Task<string> SaveJobOfferFileAsync(byte[] content, string joNumber, string fileName)
        {
            ArgumentNullException.ThrowIfNull(content);
            if (content.LongLength > MaxFileSize)
                throw new ArgumentException("The file exceeds the 10 MB limit.", nameof(content));

            using var source = new MemoryStream(content, writable: false);
            return await SaveJobOfferFileAsync(source, joNumber, fileName);
        }

        private async Task<string> SaveJobOfferFileAsync(Stream source, string joNumber, string fileName)
        {
            ValidatePathSegment(joNumber, nameof(joNumber));
            ValidatePathSegment(fileName, nameof(fileName));

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                throw new InvalidOperationException("The web root directory is not configured.");

            var directory = Path.Combine(_environment.WebRootPath, "docs", joNumber);
            Directory.CreateDirectory(directory);
            var destination = Path.GetFullPath(Path.Combine(directory, fileName));
            var temporaryPath = Path.Combine(directory, $"{Guid.NewGuid():N}.tmp");

            try
            {
                await using (var target = new FileStream(temporaryPath, FileMode.CreateNew,
                    FileAccess.Write, FileShare.None, 81920, useAsync: true))
                {
                    await source.CopyToAsync(target);
                }

                File.Move(temporaryPath, destination, overwrite: true);
                return destination;
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private static void ValidatePathSegment(string value, string parameterName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
            if (value is "." or ".." || Path.IsPathRooted(value)
                || value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || value.Contains('/') || value.Contains('\\')
                || value.EndsWith('.') || value.EndsWith(' '))
            {
                throw new ArgumentException("Use a single valid directory or file name without path separators.", parameterName);
            }
        }
    }
}
