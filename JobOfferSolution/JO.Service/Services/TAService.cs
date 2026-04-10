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
    public class TAService : ITAService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public TAService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<int> EmailCandidate(int id, int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            //Status
            var jobOffer = await context.JobOffers.FindAsync(id);
            jobOffer.MainStatus_Id = JOMainStatus.Emailed;

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.TAEmailCandidate,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = id
            };

            context.JobOffers.Update(jobOffer);
            await context.ActivityLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }

        public async Task<int> DeclineJobOffer(int id, int reasonId, string otherReason, int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            //JO Decline
            var jobOffer = await context.JobOffers.FindAsync(id);
            jobOffer.MainStatus_Id = JOMainStatus.Declined;
            jobOffer.DeclineReason_Id = reasonId;
            jobOffer.OtherDeclineReason = otherReason;

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.TATagJODecline,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = id
            };

            context.JobOffers.Update(jobOffer);
            await context.ActivityLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }

        public async Task<int> AcceptJobOffer(int id, int candidateId, int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            //JO Decline
            var jobOffer = await context.JobOffers.FindAsync(id);
            jobOffer.MainStatus_Id = JOMainStatus.Accepted;

            //Candidate
            var candidate = await context.Candidates.FindAsync(candidateId);
            candidate.CandidateStatus_Id = JOCandidateStatus.Accepted;

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.TATagJOAccept,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = id
            };

            context.JobOffers.Update(jobOffer);
            context.Candidates.Update(candidate);
            await context.ActivityLogs.AddAsync(newLog);

            return await context.SaveChangesAsync();
        }
    }
}
