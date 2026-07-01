using JO.DataModel.Entity;
using JO.DataModel.View;
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

        public async Task<List<VwCandidateApplications>> GetCandidateApplications()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateApplications
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<VwCandidateApplications> GetCandidateApplication(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateApplications.FirstOrDefaultAsync(jo => jo.Id == id);
        }

        public async Task<int> LegalEntitySetup(CandidateApplications entity)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            entity.ReferenceNumber = await CreateReferenceNumber();
            entity.CreatedAt = DateTime.Now;
            await context.CandidateApplications.AddAsync(entity);

            await context.SaveChangesAsync();
            return entity.Id;
        }

        private async Task<string> CreateReferenceNumber()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var countPlusOne = context.CandidateApplications.Count() + 1;
            return $"JO-APL-{DateTime.Now.Year}-{countPlusOne:D5}";
        }
    }
}
