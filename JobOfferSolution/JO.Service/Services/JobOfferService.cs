using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        public JobOfferService(IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<(IEnumerable<VwJobOffers> Data, int TotalCount)> SearchJobOffersAsync(
            int statusId,
            string? candidate,
            int page,
            int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.VwJobOffers
                .AsNoTracking()
                .AsQueryable();

            if (statusId != 0)
                query = query.Where(jo => jo.MainStatus_Id == statusId);

            if (!string.IsNullOrWhiteSpace(candidate))
                query = query.Where(jo =>
                    EF.Functions.Like(jo.CandidateName, $"%{candidate}%"));

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(jo => jo.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<VwJobOffers> GetJobOffer(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJobOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo => jo.Id == id);
        }

        public async Task<int> CreateJobOffer(
            int candidateId,
            int positionId,
            int departmentId,
            decimal basicSalary,
            decimal allowance,
            decimal signingBonus,
            DateTime offerDate,
            DateTime startDate,
            string remarks,
            int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            //New JO
            var newJO = new JobOffers
            {
                Allowance = allowance,
                BasicSalary = basicSalary,
                Candidate_Id = candidateId,
                Department_Id = departmentId,
                JobOfferNumber = await GenerateTransactionNumber(context),
                JobPosition_Id = positionId,
                MainStatus_Id = JOMainStatus.HRODApproval,
                OfferDate = offerDate,
                ProposedStartDate = startDate,
                Remarks = remarks,
                SigningBonus = signingBonus,
                CreatedBy = userId,
                CreatedAt = DateTime.Now
            };

            //Candidate Status
            var candidate = await context.Candidates.FindAsync(candidateId);
            candidate.CandidateStatus_Id = JOCandidateStatus.InProgress;


            // Track both inserts and persist them together.
            await context.JobOffers.AddAsync(newJO);
            context.Candidates.Update(candidate);
            await context.SaveChangesAsync();

            //Activity Log
            var newLog = new ActivityLogs
            {
                Activity_Id = JOActivity.SubmitJOtoHR,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                JobOffer_Id = newJO.Id
            };
            await context.ActivityLogs.AddAsync(newLog);
            await context.SaveChangesAsync();

            return newJO.Id;
        }

        public async Task<IEnumerable<VwReturnLogs>> GetReturnLogs(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwReturnLogs
                .AsNoTracking()
                .Where(x => x.JobOffer_Id == id)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<VwActivityLogs>> GetActivityLogs(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwActivityLogs
                .AsNoTracking()
                .Where(x => x.JobOffer_Id == id)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        private async Task<string> GenerateTransactionNumber(JobOfferDbContext context)
        {
            var year = DateTime.Now.Year;

            var lastTransaction = await context.JobOffers
                .Where(x => x.CreatedAt.Value.Year == year)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();

            int nextNumber = lastTransaction == null ? 1 : lastTransaction.Id + 1;

            return $"JO-{year}-{nextNumber.ToString().PadLeft(5, '0')}";
        }
    }
}
