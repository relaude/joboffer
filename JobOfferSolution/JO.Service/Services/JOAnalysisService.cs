using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class JOAnalysisService : IJOAnalysisService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public JOAnalysisService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> LegalEntitySetup(CandidateApplications entity)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            await context.AddAsync(entity);

            await context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
