using JO.DataModel.DTOs;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        
        private string mainUploadDirectory;

        const long MaxFileSize = 10 * 1024 * 1024;// 10 MB
        public FileUploadService(IWebHostEnvironment env, 
            IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _env = env;
            _dbContext = dbContext;

            mainUploadDirectory = $@"{_env.WebRootPath}\documents";
        }

        public async Task<int> TagSubmitted(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var jobOffer = await context.JobOffers.FindAsync(jobOfferId);
            var workFlow = await context.WorkFlow
                .Where(jo => jo.JobOfferId == jobOfferId)
                .Take(4)
                .ToListAsync();

            jobOffer.StatusId = JOStatus.Request.Submitted;

            workFlow[0].ActionId = JOStatus.Action.Done;
            workFlow[1].ActionId = JOStatus.Action.Current;
            workFlow[2].ActionId = JOStatus.Action.Next;
            workFlow[3].ActionId = JOStatus.Action.Next;

            context.JobOffers.Update(jobOffer);
            context.WorkFlow.UpdateRange(workFlow);

            return await context.SaveChangesAsync();
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
