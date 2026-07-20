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
    public class JODetailsService : IJODetailsService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public JODetailsService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JobOffers> GetJobOffer(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(id);
        }

        public async Task<Requests> GetRequest(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Requests.FindAsync(id);
        }

        public async Task<Candidates> GetCandidate(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Candidates.FindAsync(id);
        }

        public async Task<List<VwJobOfferWorkFlow>> GetJobOfferWorkFlow(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferWorkFlow
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<VwLegalEntities> GetLegalEntity(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwLegalEntities
                .FirstOrDefaultAsync(jo => jo.JobOfferId == jobOfferId);
        }

        public List<JOTabs> SetTabs(List<VwJobOfferWorkFlow> workFlow)
        {
            List<JOTabs> tabs = new();

            tabs.AddRange(
                new JOTabs { Key = "candidate", Label = "Candidate", Icon = "fas fa-user", Show = true },
                new JOTabs { Key = "email", Label = "Email Request", Icon = "fas fa-envelope", Show = true },
                new JOTabs { Key = "legal", Label = "Legal Entities", Icon = "fas fa-building", Show = workFlow[1].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "offers", Label = "Job Offers", Icon = "fas fa-file-signature", Show = workFlow[2].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "approve", Label = "Approvals", Icon = "fas fa-user-check", Show = workFlow[4].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "discuss", Label = "Discussion", Icon = "fas fa-comments", Show = workFlow[6].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "accept", Label = "Acceptance", Icon = "fas fa-handshake", Show = workFlow[7].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "negotiate", Label = "Negotiations", Icon = "fas fa-comments-dollar", Show = workFlow[8].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "letter", Label = "Offer Letter", Icon = "fas fa-envelope-open-text", Show = workFlow[9].ActionId == JOStatus.Action.Done }
            );

            return tabs;
        }
    }
}
