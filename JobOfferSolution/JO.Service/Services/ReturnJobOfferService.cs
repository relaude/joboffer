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

        public async Task<int> ReturnToTA(int id, int reasonId, int activityId, int userId, string remarks)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var jobOffer = await context.JobOffers.FindAsync(id);
            if (jobOffer is null)
                throw new InvalidOperationException($"Job offer with id {id} was not found.");

            jobOffer.MainStatus_Id = JOMainStatus.Returned;

            //Return Log
            var newReturnLog = new ReturnLogs
            {
                JobOffer_Id = id,
                ReturnReason_Id = reasonId,
                Remarks = remarks,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            //Activity Log
            var newActivityLog = new ActivityLogs
            {
                Activity_Id = activityId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = id
            };

            await context.ReturnLogs.AddAsync(newReturnLog);
            await context.ActivityLogs.AddAsync(newActivityLog);

            return await context.SaveChangesAsync();
        }

        public async Task<int> ReSubmitReturnedJO(
            int id,
            int positionId,
            int departmentId,
            decimal basicSalary,
            decimal allowance,
            decimal signingBonus,
            DateTime startDate,
            int modifiedBy)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var jobOffer = await context.JobOffers.FindAsync(id);

            //Edit JO
            jobOffer.JobPosition_Id = positionId;
            jobOffer.Department_Id = departmentId;
            jobOffer.BasicSalary = basicSalary;
            jobOffer.Allowance = allowance;
            jobOffer.SigningBonus = signingBonus;
            jobOffer.ProposedStartDate = startDate;

            jobOffer.MainStatus_Id = JOMainStatus.HRODApproval;
            jobOffer.ModifiedBy = modifiedBy;
            jobOffer.ModifiedAt = DateTime.Now;

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.SubmitJOtoHR,
                CreatedAt = DateTime.Now,
                CreatedBy = modifiedBy,
                JobOffer_Id = id
            };

            context.JobOffers.Update(jobOffer);
            await context.ActivityLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }
    }
}
