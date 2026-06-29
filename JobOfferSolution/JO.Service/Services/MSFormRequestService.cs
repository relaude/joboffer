using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class MSFormRequestService : IMSFormRequestService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public MSFormRequestService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> TagMSFormRequestAsSubmitted(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var msFormRequest = await context.CandidateMSFormRequests.FindAsync(id);
            msFormRequest.StatusId = FormRequestStatus.Submitted;
            context.CandidateMSFormRequests.Update(msFormRequest);

            return await context.SaveChangesAsync();
        }

        public async Task<List<VwCandidateMSFormRequests>> GetMSFormRequests()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateMSFormRequests
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<VwCandidateMSFormRequests> GetMSFormRequest(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateMSFormRequests.FirstOrDefaultAsync(jo => jo.Id == id);
        }
    }
}
