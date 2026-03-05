using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public ReportService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<MainStatusCount>> GetMainStatusCount()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var statusCounts = await context.VwJobOffers
                .AsNoTracking()
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Total = g.Count(),

                    New = g.Count(x =>
                        x.MainStatus_Id == JOMainStatus.New),

                    Approved = g.Count(x =>
                        x.MainStatus_Id == JOMainStatus.HRODApproved ||
                        x.MainStatus_Id == JOMainStatus.DHApproved),

                    Emailed = g.Count(x =>
                        x.MainStatus_Id == JOMainStatus.Emailed),

                    Accepted = g.Count(x =>
                        x.MainStatus_Id == JOMainStatus.Accepted),

                    Declined = g.Count(x =>
                        x.MainStatus_Id == JOMainStatus.Declined)
                })
                .FirstOrDefaultAsync();

            if (statusCounts == null)
                return Enumerable.Empty<MainStatusCount>();

            return new List<MainStatusCount>
            {
                new() { StatusName = "Total",    StatusCount = statusCounts.Total },
                new() { StatusName = "New",    StatusCount = statusCounts.New },
                new() { StatusName = "Approved", StatusCount = statusCounts.Approved },
                new() { StatusName = "Emailed", StatusCount = statusCounts.Emailed },
                new() { StatusName = "Accepted", StatusCount = statusCounts.Accepted },
                new() { StatusName = "Declined", StatusCount = statusCounts.Declined }
            };
        }
    }
}
