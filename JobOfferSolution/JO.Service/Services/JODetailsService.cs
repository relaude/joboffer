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

        public async Task<List<VwTAPartnerDboxCandidates>> GetVwTAPartnerDboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwTAPartnerDboxCandidates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<VwTALeadDboxCandidates>> GetVwTALeadDboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwTALeadDboxCandidates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int templateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwPckgTempHasItms
                .AsNoTracking()
                .Where(jo => jo.TempId == templateId)
                .ToListAsync();
        }

        public async Task<List<CompensationPackage>> GetCompensationPackage(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompensationPackage.AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetVwJODboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetPEHeadForReviewVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //14 = For PE Head Review
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 14
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetTALeadForReviewVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //3 = For TA Lead Review
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 3
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetHRODHeadForApprovalVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //6 = For HROD Head Approval
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 6
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetDivHeadL1ForApprovalVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //5 = For Division Head Approval
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 5
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetDivHeadL2ForApprovalVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //13 = For Division Head L2 Approval
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 13
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwJODboxCandidates>> GetPresidentForApprovalVwJODboxCandidates(int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var actionLogs = await context.VwJOActionLogs.AsNoTracking()
                .Where(jo => jo.ActionBy == userId)
                .ToListAsync();

            var joIds = actionLogs.Select(jo => jo.JobOfferId).Distinct().ToList();
            //7 = For President Approval
            return await context.VwJODboxCandidates
                .AsNoTracking()
                .Where(jo=>jo.WorkFlowId == 7
                    || joIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<JobOffers> GetJobOffer(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(id);
        }

        public async Task<List<VwJobOffers>> GetJobOffers()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOffers.ToListAsync();
        }

        public async Task<List<VwDiscussions>> GetDiscussions(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDiscussions
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .OrderByDescending(jo => jo.DiscussAt)
                .ToListAsync();
        }

        public async Task<List<VwApprovals>> GetVwApprovals(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwApprovals
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<SalaryBandStatus>> GetSalaryBandStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.SalaryBandStatus
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Proposal>> GetProposal(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Proposal
                .AsNoTracking()
                .Where(jo=>jo.JobOfferId == jobOfferId)
                .ToListAsync();
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
                .AsNoTracking()
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

            int[] doneCurrent = { JOStatus.Action.Done, JOStatus.Action.Current };

            tabs.AddRange(
                new JOTabs { Key = "candidate", Label = "Candidate", Icon = "fas fa-user", Show = true },
                new JOTabs { Key = "email", Label = "Email Request", Icon = "fas fa-envelope", Show = true },
                new JOTabs { Key = "docs", Label = "Documents", Icon = "fas fa-file-alt", Show = true },
                new JOTabs { Key = "legal", Label = "Company", Icon = "fas fa-building", Show = workFlow[1].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "offers", Label = "Job Offers", Icon = "fas fa-file-signature", Show = workFlow[2].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "approve", Label = "Approvals", Icon = "fas fa-user-check", Show = doneCurrent.Contains(workFlow[4].ActionId.GetValueOrDefault()) },
                new JOTabs { Key = "discuss", Label = "Discussion", Icon = "fas fa-comments", Show = doneCurrent.Contains(workFlow[6].ActionId.GetValueOrDefault()) },
                new JOTabs { Key = "accept", Label = "Acceptance", Icon = "fas fa-handshake", Show = workFlow[7].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "negotiate", Label = "Negotiations", Icon = "fas fa-comments-dollar", Show = workFlow[8].ActionId == JOStatus.Action.Done },
                new JOTabs { Key = "letter", Label = "Offer Letter", Icon = "fas fa-envelope-open-text", Show = workFlow[9].ActionId == JOStatus.Action.Done }
            );

            return tabs;
        }

        public List<JOTabs> SetTabs()
        {
            List<JOTabs> tabs = new();

            tabs.AddRange(
                new JOTabs { Key = "candidate", Label = "Candidate", Icon = "fas fa-user", Show = true },
                new JOTabs { Key = "docs", Label = "Documents", Icon = "fas fa-file-alt", Show = true },
                new JOTabs { Key = "legal", Label = "Company", Icon = "fas fa-building", Show = true },
                new JOTabs { Key = "offers", Label = "Offers", Icon = "fas fa-file-signature", Show =true},
                new JOTabs { Key = "approve", Label = "Approvals", Icon = "fas fa-user-check", Show = true },
                new JOTabs { Key = "discuss", Label = "Discussion", Icon = "fas fa-comments", Show = true },
                new JOTabs { Key = "letter", Label = "Offer Letter", Icon = "fas fa-envelope-open-text", Show = true }
            );

            return tabs;
        }

        public List<JOTabs> SetNewOfferTabs()
        {
            List<JOTabs> tabs = new();

            tabs.AddRange(
                new JOTabs { Key = "candidate", Label = "Candidate", Icon = "fas fa-user", Show = true },
                new JOTabs { Key = "offers", Label = "Offers", Icon = "fas fa-file-signature", Show = true },
                new JOTabs { Key = "docs", Label = "Documents", Icon = "fas fa-file-alt", Show = true }
            );

            return tabs;
        }
    }
}
