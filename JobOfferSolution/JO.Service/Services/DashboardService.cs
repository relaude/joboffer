using JO.DataModel.DTOs;
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
    public class DashboardService : IDashboardService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public DashboardService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<VwJobOffers>> GetAllJobOffers()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJobOffers
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<VwJobOffers>> GetAllJobOffers(int statusId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJobOffers
                    .AsNoTracking()
                    .Where(jo=>jo.MainStatus_Id == statusId)
                    .ToListAsync();
        }
    }
}
