using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Persistence.Repositories.Contracts;
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

        public async Task<VwJobOfferTransactions> GetTransactionDetails(int id)
        {
            //return await _vwTransaction.GetByIdAsync(id);
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwJobOfferTransactions
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<VwTransactionAttachments>> GetSubmittedFiles(int id)
        {
            //return await _vwAttachment.FindAsync(vw=>vw.Transaction_Id == id);
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwTransactionAttachments
                                .AsNoTracking()
                                .Where(x => x.Transaction_Id == id)
                                .ToListAsync();
        }
    }
}
