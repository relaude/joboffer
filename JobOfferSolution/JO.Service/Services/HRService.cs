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
    public class HRService : IHRService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public HRService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> ApproveJobOffer(int id, int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            //Status
            var jobOffer = await context.JobOffers.FindAsync(id);
            jobOffer.MainStatus_Id = JOMainStatus.HRODApproved;

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.HRApproved,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = id
            };

            context.JobOffers.Update(jobOffer);
            await context.ActivityLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }
    }
}
