using JO.DataModel.DTOs;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private string mainUploadDirectory;

        const long MaxFileSize = 10 * 1024 * 1024;// 10 MB
        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
            mainUploadDirectory = $@"{_env.WebRootPath}\documents";
        }

        public string GetRootPath()
        {
            return _env.ContentRootPath;
        }

        public string GetWwwRootPath()
        {
            return _env.WebRootPath;
        }
        public async Task<byte[]> ReadFileAsync(IBrowserFile file)
        {
            await using var stream = file.OpenReadStream(MaxFileSize);
            await using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        public async Task<string> CreateFilePath(string transactionNumber, string fileName)
        {
            CreateUploadMainDirectory();
            string transactionDirectory = CreateUploadSubDirectory(transactionNumber);
            return Path.Combine(transactionDirectory, fileName);
        }

        private void CreateUploadMainDirectory()
        {
            if (!Directory.Exists(mainUploadDirectory))
            { Directory.CreateDirectory(mainUploadDirectory); }
        }

        private string CreateUploadSubDirectory(string folderName)
        {
            string newDirectory = Path.Combine(mainUploadDirectory, folderName);
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }
            return newDirectory;
        }
    }
}
