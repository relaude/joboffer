using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JO.Service.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        private readonly IFileUploadService _fileService;
        public CandidateService(
            IDbContextFactory<JobOfferDbContext> contextFactory,
            IFileUploadService fileService)
        {
            _contextFactory = contextFactory;
            _fileService = fileService;
        }

        public async Task<VwCandidates> GetCandidate(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<IEnumerable<VwCandidates>> GetAllCandidates()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates.AsNoTracking().ToListAsync();
        }

        public async Task<int> NewTransactionAsync(
            string name,
            string email,
            List<AttachmentDto> attachments)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await using var dbTransaction = await context.Database.BeginTransactionAsync();

            try
            {
                //Create Candidate
                var candidate = new Candidates
                {
                    FirstName = name,
                    Email = email,
                    CreatedAt = DateTime.Now
                };

                await context.Candidates.AddAsync(candidate);
                await context.SaveChangesAsync();

                //Create Job Offer Transaction
                var transaction = new JobOfferTransactions
                {
                    TransactionNumber = await GenerateTransactionNumber(context),
                    MainStatus_Id = 1,
                    Candidate_Id = candidate.Id,
                    CreatedAt = DateTime.Now
                };

                await context.JobOfferTransactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                //Create Attachments
                foreach (var item in attachments)
                    item.FilePath = await _fileService.CreateFilePath(transaction.TransactionNumber, item.FileName);

                var attachmentEntities = attachments.Select(a =>
                    new TransactionAttachments
                    {
                        Transaction_Id = transaction.Id,
                        FileType_Id = a.FileType_Id,
                        FileName = a.FileName,
                        FilePath = a.FilePath,
                        CreatedAt = DateTime.Now
                    }).ToList();

                await context.TransactionAttachments.AddRangeAsync(attachmentEntities);
                await context.SaveChangesAsync();
                
                //Upload Files
                foreach (var item in attachments)
                    await File.WriteAllBytesAsync(item.FilePath, item.FileBytes);

                await dbTransaction.CommitAsync();

                return transaction.Id;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        private async Task<string> GenerateTransactionNumber(JobOfferDbContext context)
        {
            var year = DateTime.Now.Year;

            var lastTransaction = await context.JobOfferTransactions
                .Where(x => x.CreatedAt.Value.Year == year)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            int nextNumber = lastTransaction == null ? 1 : lastTransaction.Id + 1;

            return $"JO-{year}-{nextNumber.ToString().PadLeft(5, '0')}";
        }

        private async Task UploadFiles(List<AttachmentDto> attachments)
        {

        }
    }
}
