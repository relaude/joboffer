using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class JOLogsService : IJOLogsService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public JOLogsService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwJOActionLogs>> GetVwJOActionLogs(int jobOfferId)
        {
            if (jobOfferId <= 0)
                return [];

            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJOActionLogs
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .OrderByDescending(jo => jo.ActionAt)
                .ThenByDescending(jo => jo.Id)
                .ToListAsync();
        }
    }
}
