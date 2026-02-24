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
            string remarks)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var newJO = new JobOffers
            {
                CreatedAt = DateTime.Now,
                Allowance = allowance,
                BasicSalary = basicSalary,
                Candidate_Id = candidateId,
                Department_Id = departmentId,
                JobOfferNumber = await GenerateTransactionNumber(context),
                JobPosition_Id = positionId,
                MainStatus_Id = JOMainStatus.ForApproval,
                OfferDate = offerDate,
                ProposedStartDate = startDate,
                Remarks = remarks,
                SigningBonus = signingBonus
            };

            await context.JobOffers.AddAsync(newJO);
            await context.SaveChangesAsync();

            return newJO.Id;
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
