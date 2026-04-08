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
    public class ReturnJobOfferService : IReturnJobOfferService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public ReturnJobOfferService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> ReturnToTA(int id, int reasonId, int userId, string remarks)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var jobOffer = await context.JobOffers.FindAsync(id);
            if (jobOffer is null)
                throw new InvalidOperationException($"Job offer with id {id} was not found.");

            jobOffer.MainStatus_Id = JOMainStatus.Returned;

            var newLog = new ReturnLogs
            {
                JobOffer_Id = id,
                ReturnReason_Id = reasonId,
                Remarks = remarks,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            await context.ReturnLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }
    }
}
