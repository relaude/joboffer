using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class CandidateDiscussionService : ICandidateDiscussionService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public CandidateDiscussionService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VwJobOfferAnalysis> GetJobOfferAnalysis(int applicationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferAnalysis.FirstOrDefaultAsync(jo => jo.CandidateApplicationId == applicationId) ?? new();
        }
    }
}
