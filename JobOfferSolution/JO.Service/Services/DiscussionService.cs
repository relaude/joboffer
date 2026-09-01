using JO.DataModel.DTOs;
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
    public class DiscussionService : IDiscussionService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public DiscussionService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
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
            if (!dto.ProposalId.HasValue)
                throw new ArgumentException("A proposal must be selected before saving a discussion.", nameof(dto));

            await using var context = await _dbContext.CreateDbContextAsync();

            var joCompen = await context.JOCompanyCompensation.FindAsync(dto.ProposalId.Value);
            if (joCompen is null || joCompen.JobOfferId != dto.JobOfferId
                || !(joCompen.OptionNumber > 0) || joCompen.Declined == true)
                throw new ArgumentException("The selected proposal is not available for this job offer.", nameof(dto));

            var newDiscussion = new Discussions
            {
                JobOfferId = dto.JobOfferId,
                ProposalId = dto.ProposalId,
                StepId = dto.StepId,
                ChannelId = dto.ChannelId,
                ResponseId = dto.ResponseId,
                Comments = dto.Comments,
                FeedBack = dto.FeedBack,
                DiscussAt = dto.DiscussAt,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };
            await context.Discussions.AddAsync(newDiscussion);

            joCompen.Declined = dto.ResponseId == 5; //Declined
            joCompen.Accepted = dto.ResponseId == 3; //Accepted
            context.JOCompanyCompensation.Update(joCompen);

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
