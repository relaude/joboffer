using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class MSFormRequestService : IMSFormRequestService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public MSFormRequestService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwJobOfferWorkFlow>> GetJobOfferWorkFlow(int requestId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferWorkFlow
                .Where(jo => jo.CandidateMSFormRequestId == requestId)
                .OrderBy(jo => jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<int> TagMSFormRequestAsSubmitted(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var msFormRequest = await context.Requests.FindAsync(id);
            //msFormRequest.StatusId = JOStatus.FormRequestStatus.Submitted;

            var joWorkFlows = await context.JobOfferWorkFlow
                .Where(jo=>jo.CandidateMSFormRequestId==id)
                .Take(4)
                .ToListAsync();

            joWorkFlows[0].WorkFlowActionId = JOStatus.Action.Done;
            joWorkFlows[1].WorkFlowActionId = JOStatus.Action.Current;
            joWorkFlows[2].WorkFlowActionId = JOStatus.Action.Next;
            joWorkFlows[3].WorkFlowActionId = JOStatus.Action.Next;

            context.Requests.Update(msFormRequest);
            context.JobOfferWorkFlow.UpdateRange(joWorkFlows);
            return await context.SaveChangesAsync();
        }

        public async Task<List<VwCandidateMSFormRequests>> GetMSFormRequests()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateMSFormRequests
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<VwCandidateMSFormRequests> GetMSFormRequest(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateMSFormRequests.FirstOrDefaultAsync(jo => jo.Id == id);
        }
    }
}
