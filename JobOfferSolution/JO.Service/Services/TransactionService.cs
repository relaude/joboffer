using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;

        public TransactionService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<VwJobOfferTransactions>> GetAllTransaction()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwJobOfferTransactions
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<VwJobOfferTransactions> GetTransactionDetails(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwJobOfferTransactions
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<VwTransactionAttachments>> GetSubmittedFiles(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwTransactionAttachments
                                .AsNoTracking()
                                .Where(x => x.Transaction_Id == id)
                                .ToListAsync();
        }
        
        public async Task<IEnumerable<JobOfferPackages>> GetPackages(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferPackages
                                .AsNoTracking()
                                .Where(x => x.Transaction_Id == id)
                                .OrderBy(x => x.Priority)
                                .ToListAsync();
        }

        public async Task<int> SetTransactionStatus(int id, int statusId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var joTransaction = await context.JobOfferTransactions.FindAsync(id);
            joTransaction.MainStatus_Id = statusId;

            context.JobOfferTransactions.Update(joTransaction);
            return await context.SaveChangesAsync();
        }
    }
}
