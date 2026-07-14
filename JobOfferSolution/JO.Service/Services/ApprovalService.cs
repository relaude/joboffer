using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public ApprovalService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveDivisionHeadApprovals(List<DHJOProposal> dhProposal)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            int?[] IDs = dhProposal.Select(jo=>jo.JobOfferProposalId).ToArray();
            var cntDecisions = await context.DHJOProposal
                    .AsNoTracking()
                    .Where(jo => IDs.Contains(jo.JobOfferProposalId))
                    .CountAsync();

            if (cntDecisions > 0)
                return 0;

            await context.DHJOProposal.AddRangeAsync(dhProposal);
            return await context.SaveChangesAsync();
        }
    }
}
