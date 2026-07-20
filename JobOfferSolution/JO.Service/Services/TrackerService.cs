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
    public class TrackerService : ITrackerService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public TrackerService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwJobOffers>> GetJobOffers()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOffers.ToListAsync();
        }
    }
}
