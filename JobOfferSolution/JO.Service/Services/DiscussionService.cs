using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class DiscussionService : IDiscussionService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public DiscussionService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ForNegotiation(JobOffers jobOffer,
            JOAnalysis joAnalysis,
            VwDboxCandidates candidate,
            List<JOCompanyCompensation> joCompanyCompensation,
            int options, 
            int createdBy)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            int lastOptionNumber = joCompanyCompensation
                .Where(jo => jo.JobOfferId == jobOffer.Id)
                .Max(jo => jo.OptionNumber.GetValueOrDefault());

            int cmpnyCmpnstnId = joCompanyCompensation.FirstOrDefault().CmpnyCmpnstnId.GetValueOrDefault();

            //update status
            jobOffer.WorkFlowId = 11;//For Negotiation
            foreach (var item in joCompanyCompensation)
            {
                item.Declined = true;
                item.Accepted = false;
                item.ForNegotiation = false;
            }

            context.JobOffers.Update(jobOffer);
            context.JOCompanyCompensation.UpdateRange(joCompanyCompensation);
            await context.SaveChangesAsync();


            //Options
            List<JOCompanyCompensation> joCompanyCompensationsB = new();
            for (int i = 1; i <= options; i++)
            {
                joCompanyCompensationsB.Add(new JOCompanyCompensation
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                    JobOfferId = jobOffer.Id,
                    JOAnalysisId = joAnalysis.Id,
                    CSGId = candidate.CSGId,
                    OptionNumber = i + lastOptionNumber,
                    CurrentSalary = candidate.CurrentMonthlyBasicSalary,
                    ForNegotiation = true,
                    CmpnyCmpnstnId =cmpnyCmpnstnId
                });
            }

            await context.JOCompanyCompensation.AddRangeAsync(joCompanyCompensationsB);
            await context.SaveChangesAsync();

            List<JOCompanyCompensationItems> joCmpnyCompensationItemsB = new();
            var compItems = await context.CompensationItems.ToListAsync();
            foreach (var itemA in joCompanyCompensationsB)
            {
                foreach (var itemB in compItems)
                {
                    joCmpnyCompensationItemsB.Add(new JOCompanyCompensationItems
                    {
                        JobOfferId = jobOffer.Id,
                        JOCmpnyCmpnstnId = itemA.Id,
                        ItemId = itemB.Id
                    });
                }
            }

            await context.JOCompanyCompensationItems.AddRangeAsync(joCmpnyCompensationItemsB);
            await context.SaveChangesAsync();

            //
        }

        public async Task<List<JODeclineReason>> GetJODeclineReason()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JODeclineReason.AsNoTracking().ToListAsync();
        }

        public async Task<List<DiscussionStatus>> GetDiscussionStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DiscussionStatus.AsNoTracking().ToListAsync();
        }

        public async Task<JobOffers> GetJobOffer(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(jobOfferId);
        }

        public async Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJODboxCandidates.FirstOrDefaultAsync(jo => jo.Id == jobOfferId);
        }

        public async Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.FirstOrDefaultAsync(jo => jo.Id == candidateId);
        }

        public async Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOCompanyCompensation
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<int> TagAsAccepted(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            //joboffer
            var jobOffer = await context.JobOffers.FindAsync(jobOfferId);
            jobOffer.StatusId = JOStatus.Application.Accepted;

            //workflow
            var joWorkFlow = await context.WorkFlow
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();

            joWorkFlow[6].ActionId = JOStatus.Action.Done;
            joWorkFlow[7].ActionId = JOStatus.Action.Done;
            joWorkFlow[9].ActionId = JOStatus.Action.Current;

            //updating...
            context.JobOffers.Update(jobOffer);
            context.WorkFlow.UpdateRange(joWorkFlow);

            return await context.SaveChangesAsync();
        }

        public async Task<List<VwDiscussions>> GetDiscussions(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDiscussions
                .Where(jo => jo.JobOfferId == jobOfferId)
                .OrderByDescending(jo=>jo.DiscussAt)
                .ToListAsync();
        }

        public async Task<int> SaveDiscussion(Discussions discussion)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            discussion.CreatedAt = DateTime.Now;
            await context.Discussions.AddAsync(discussion);
            await context.SaveChangesAsync();

            return discussion.Id;
        }

        public async Task<int> SaveDiscussion(DiscussionDto dto)
        {
            if (!dto.StatusId.HasValue)
                throw new ArgumentException("A status must be selected before saving a discussion.", nameof(dto));

            if (!dto.ProposalId.HasValue)
                throw new ArgumentException("A proposal must be selected before saving a discussion.", nameof(dto));

            await using var context = await _dbContext.CreateDbContextAsync();

            if (!await context.DiscussionStatus.AnyAsync(status => status.Id == dto.StatusId.Value))
                throw new ArgumentException("The selected discussion status is invalid.", nameof(dto));

            var joCompen = await context.JOCompanyCompensation.FindAsync(dto.ProposalId.Value);
            if (joCompen is null || joCompen.JobOfferId != dto.JobOfferId
                || !(joCompen.OptionNumber > 0) || joCompen.Declined == true)
                throw new ArgumentException("The selected proposal is not available for this job offer.", nameof(dto));

            var newDiscussion = new Discussions
            {
                JobOfferId = dto.JobOfferId,
                StatusId = dto.StatusId,
                ProposalId = dto.ProposalId,
                DeclineReasonId = dto.DeclineReasonId,
                DeclineRemarks = dto.DeclineRemarks,
                Comments = dto.Comments,
                FeedBack = dto.FeedBack,
                DiscussAt = dto.DiscussAt,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };
            await context.Discussions.AddAsync(newDiscussion);

            if(dto.StatusId == 3 || dto.StatusId == 4)
            {
                joCompen.Declined = dto.StatusId == 4; //Declined Offer
                joCompen.Accepted = dto.StatusId == 3; //Accepted Offer
                context.JOCompanyCompensation.Update(joCompen);
            }

            await context.SaveChangesAsync();
            return newDiscussion.Id;
        }

        public async Task<List<CandResponse>> GetCandResponse()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CandResponse.AsNoTracking().OrderBy(jo => jo.DisplayOrder).ToListAsync();
        }

        public async Task<List<DiscussSteps>> GetDiscussSteps()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DiscussSteps.AsNoTracking().OrderBy(jo=>jo.DisplayOrder).ToListAsync();
        }

        public async Task<List<ChannelTypes>> GetChannelTypes()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.ChannelTypes.AsNoTracking().ToListAsync();
        }

        public async Task<List<Proposal>> GetApprovedProposal(int jobOfferId)
        {
            List<Proposal> proposal = new();

            await using var context = await _dbContext.CreateDbContextAsync();

            var proposalIDs = await context.Approvals
                .Where(jo => jo.JobOfferId == jobOfferId
                    && jo.StatusId == JOStatus.Proposal.Approve)
                .Select(jo => jo.ProposalId)
                .ToListAsync();

            if (proposalIDs.Any())
            {
                proposal = await context.Proposal
                    .Where(jo => proposalIDs.Contains(jo.Id))
                    .OrderBy(jo => jo.OptionNum)
                    .ToListAsync();
            }

            return proposal;
        }
    }
}
