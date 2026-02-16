using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
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
        public CandidateService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
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
                    Name = name,
                    Email = email
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
                if (attachments != null && attachments.Any())
                {
                    var attachmentEntities = attachments.Select(a =>
                        new TransactionAttachments
                        {
                            Transaction_Id = transaction.Id,
                            FileType_Id = a.FileType_Id,
                            FileName = a.FileName,
                            FilePath = a.FilePath
                        }).ToList();

                    await context.TransactionAttachments.AddRangeAsync(attachmentEntities);
                    await context.SaveChangesAsync();
                }

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
                .Where(x => x.CreatedAt.Year == year)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            int nextNumber = lastTransaction == null ? 1 : lastTransaction.Id + 1;

            return $"JO-{year}-{nextNumber.ToString().PadLeft(5, '0')}";
        }
    }
}
