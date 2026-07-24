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
    public class ApprovalService : IApprovalService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public ApprovalService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> DHApprovals(List<ProposalDto> joProposal)
        {
            var dhApprovals = joProposal.Select(jo=> new JO.DataModel.Entity.Approvals { 
                JobOfferId = jo.JobOfferId,
                ProposalId = jo.Id,
                TypeId = JOApprovers.DivisionHead,
                StatusId = jo.ApproveStatusId,
                Comments = jo.Comments,
                ApproveBy = jo.ApproveBy,
                ApproveAt = DateTime.Now
            }).ToList();

            int jobOfferId = joProposal.FirstOrDefault().JobOfferId.GetValueOrDefault();
            bool hasEscalate = joProposal.Any(jo => jo.Escalate == true);

            await using var context = await _dbContext.CreateDbContextAsync();

            var jobOffer = await context.JobOffers.FindAsync(jobOfferId);
            jobOffer.StatusId = hasEscalate ? JOStatus.Application.DHApproved : JOStatus.Application.Approved;

            var workFlow = await context.WorkFlow
                .Where(jo=>jo.JobOfferId == jobOfferId)
                .ToListAsync();

            workFlow[4].ActionId = JOStatus.Action.Done; //Approval
            workFlow[5].ActionId = hasEscalate ? JOStatus.Action.Current : JOStatus.Action.Open; //Escalation
            workFlow[6].ActionId = hasEscalate ? JOStatus.Action.Next : JOStatus.Action.Current; //Discussion
            workFlow[7].ActionId = hasEscalate ? JOStatus.Action.Open : JOStatus.Action.Next; //Acceptance

            //updating...
            await context.Approvals.AddRangeAsync(dhApprovals);
            context.JobOffers.Update(jobOffer);
            context.WorkFlow.UpdateRange(workFlow);

            return await context.SaveChangesAsync();
        }

        public async Task<List<ProposalDto>> GetProposalDto(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var salaryBand = await context.SalaryBandStatus
                .AsNoTracking()
                .ToListAsync();

            var joProposals = await context.Proposal
                .AsNoTracking()
                .Where(jo=>jo.JobOfferId == jobOfferId)
                .OrderBy(jo=>jo.OptionNum)
                .Select(jo=> new ProposalDto { 
                    Id = jo.Id,
                    JobOfferId = jo.JobOfferId,
                    SalaryBandId = jo.SalaryBandId,
                    OptionNum = jo.OptionNum,
                    CurrentSalary = jo.CurrentSalary,
                    ProposeSalary = jo.ProposeSalary,
                    CompaRatio = jo.CompaRatio,
                    Increase = jo.Increase,
                    Annual = jo.Annual,
                    PackageId = jo.PackageId,
                    Recommend = jo.Recommend,
                    StatusId = jo.StatusId,
                    Escalate = jo.Escalate,
                    ApproveStatusId = JOStatus.Proposal.New
                }).ToListAsync();

            foreach (var item in joProposals)
                item.StatusName = salaryBand.FirstOrDefault(x => x.Id == item.StatusId)?.StatusName ?? "";

            return joProposals;
        }
    }
}
