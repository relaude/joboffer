using JO.DataModel.Entity;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class ForNegotiationService : IForNegotiationService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public ForNegotiationService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SubmitForApproval(
            JobOffers jobOffer,
            JOAnalysis joAnalysis,
            List<JOCompanyCompensation> joCompanyCompensation,
            List<JOCompanyCompensationItems> joCompanyCompensationItems,
            int userId,
            string taPartnerRemarks)
        {
            var forNegoJOCompanyCompensation = joCompanyCompensation.Where(jo => jo.ForNegotiation == true).ToList();
            var cmpnyCmpnstnIds = forNegoJOCompanyCompensation.Select(jo => jo.CmpnyCmpnstnId).ToList();
            var forNegoJOCompanyCompensationItems = joCompanyCompensationItems
                .Where(jo => cmpnyCmpnstnIds.Contains(jo.JOCmpnyCmpnstnId.GetValueOrDefault()))
                .ToList();

            await using var context = await _dbContext.CreateDbContextAsync();

            //JobOffers
            jobOffer.WorkFlowId = 3;//For Review
            jobOffer.ModifiedBy = userId;
            jobOffer.ModifiedAt = DateTime.Now;
            //jobOffer.CmpnyCmpnstnId = selectedCmpnyCmpnstnId;
            jobOffer.Escalate = forNegoJOCompanyCompensation.Any(jo => jo.Escalate == true);
            jobOffer.OfferRangeId = forNegoJOCompanyCompensation.Max(jo => jo.OfferRangeId);

            //JOAnalysis
            joAnalysis.ModifiedBy = userId;
            joAnalysis.ModifiedAt = DateTime.Now;

            //JOActionLogs
            JOActionLogs newLog = new JOActionLogs
            {
                JobOfferId = jobOffer.Id,
                RoleId = 1,//TA Partner
                ActionId = 2,//Prepared
                ActionAt = DateTime.Now,
                ActionBy = userId,
                Remarks = taPartnerRemarks
            };

            //joCompanyCompensation
            foreach (var joCompensation in forNegoJOCompanyCompensation)
            {
                joCompensation.ModifiedBy = userId;
                joCompensation.ModifiedAt = DateTime.Now;
            }

            //var candidate = await context.DboxCandidates.FindAsync(candidateId);
            //candidate.StatusId = 3;//JO Created

            //JOApprovalFlow
            List<JOApprovalFlow> newApprovalFlow = new();
            newApprovalFlow.Add(
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 1, IsAproved = true } //TA Partner
                );

            if (jobOffer.OfferRangeId == 1)
            {
                newApprovalFlow.AddRange(
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 2 }, //TA Lead
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 5 } //Division Head Approver L1
                );
            }

            if (jobOffer.OfferRangeId == 2)
            {
                newApprovalFlow.AddRange(
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 2 }, //TA Lead
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 3 }, //PE Head
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 5 }, //Division Head Approver L1
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 4 } //HROD Head Approver
                );
            }

            if (jobOffer.OfferRangeId == 3)
            {
                newApprovalFlow.AddRange(
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 2 }, //TA Lead
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 3 }, //PE Head
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 5 }, //Division Head Approver L1
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 4 }, //HROD Head Approver
                    new JOApprovalFlow { JobOfferId = jobOffer.Id, RoleId = 7 } //President
                );
            }

            await context.JOActionLogs.AddAsync(newLog);
            await context.JOApprovalFlow.AddRangeAsync(newApprovalFlow);

            context.JobOffers.Update(jobOffer);
            context.JOAnalysis.Update(joAnalysis);
            //context.DboxCandidates.Update(candidate);
            context.JOCompanyCompensation.UpdateRange(forNegoJOCompanyCompensation);
            context.JOCompanyCompensationItems.UpdateRange(forNegoJOCompanyCompensationItems);

            await context.SaveChangesAsync();

            return jobOffer.Id;
        }

        public async Task<int> SaveAnalysis(
            List<JOCompanyCompensation> joCompanyCompensation,
            List<JOCompanyCompensationItems> joCompanyCompensationItems,
            JOAnalysis joAnalysis,
            JobOffers jobOffer,
            int userId)
        {
            joAnalysis.ModifiedBy = userId;
            joAnalysis.ModifiedAt = DateTime.Now;

            jobOffer.ModifiedBy = userId;
            jobOffer.ModifiedAt = DateTime.Now;

            var forNegoJOCompanyCompensation = joCompanyCompensation.Where(jo => jo.ForNegotiation == true).ToList();
            var cmpnyCmpnstnIds = forNegoJOCompanyCompensation.Select(jo => jo.CmpnyCmpnstnId).ToList();
            var forNegoJOCompanyCompensationItems = joCompanyCompensationItems
                .Where(jo => cmpnyCmpnstnIds.Contains(jo.JOCmpnyCmpnstnId.GetValueOrDefault()))
                .ToList();

            foreach (var joCompensation in forNegoJOCompanyCompensation)
            {
                joCompensation.ModifiedBy = userId;
                joCompensation.ModifiedAt = DateTime.Now;
            }

            await using var context = await _dbContext.CreateDbContextAsync();

            context.JOAnalysis.Update(joAnalysis);
            context.JobOffers.Update(jobOffer);

            context.JOCompanyCompensation.UpdateRange(forNegoJOCompanyCompensation);
            context.JOCompanyCompensationItems.UpdateRange(forNegoJOCompanyCompensationItems);

            return await context.SaveChangesAsync();
        }
    }
}
