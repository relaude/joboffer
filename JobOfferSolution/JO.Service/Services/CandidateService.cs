using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Extensions;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using static Dapper.SqlMapper;

namespace JO.Service.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IEmailService _email;
        public CandidateService(IDbContextFactory<JobOfferDbContext> dbContext, IEmailService email)
        {
            _dbContext = dbContext;
            _email = email;
        }

        public async Task<string> GetCandidateLink(VwDboxCandidates candidate)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var jobOffer = await context.JobOffers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo=>jo.CandidateId==candidate.Id);

            if(candidate.StatusId == 2)
            {
                return $"{JORoutes.TA.Analysis}/{jobOffer.Id}";
            }

            return $"{JORoutes.TA.Candidate}/{candidate.Id}";
        }

        public async Task<List<DboxCandidates>> GetDboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.DboxCandidates.AsNoTracking().ToListAsync();
        }

        public async Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.FirstOrDefaultAsync(jo=>jo.Id==candidateId);
        }

        public async Task<List<VwDboxCandidates>> GetVwDboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.AsNoTracking().ToListAsync();
        }

        public async Task<CandidateResponses> GetCandidateResponse(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CandidateResponses.FindAsync(id);
        }

        public async Task<List<CandidateResponses>> GetCandidateResponses()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CandidateResponses.AsNoTracking().ToListAsync();
        }

        public async Task<List<Candidates>> GetCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Candidates.ToListAsync();
        }

        public async Task<Candidates> GetCandidate(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.Candidates.FindAsync(id);
        }

        public async Task<int> CreateJobOffer(VwDboxCandidates candidate, int options, int createdBy)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            int countJO = await context.JobOffers.CountAsync() + 1;
            var newJO = new JobOffers
            {
                RefNum = $"JO-{DateTime.Now.Year}-{candidate.DboxRefNum}-{countJO:D5}",
                CompanyId = candidate.CompanyId,
                DivisionId = candidate.DivisionId,
                DepartmentId = candidate.DepartmentId,
                CandidateId = candidate.Id,
                Options = options,
                WorkFlowId = 1, //Draft
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };
            await context.JobOffers.AddAsync(newJO);
            await context.SaveChangesAsync();

            //JOAnalysis
            JOAnalysis newAnalysis = new JOAnalysis
            {
                JobOfferId = newJO.Id,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };
            await context.JOAnalysis.AddAsync(newAnalysis);
            await context.SaveChangesAsync();

            //Candidate Current
            JOCompanyCompensation joCompanyCompensationA = new JOCompanyCompensation
            {
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
                JobOfferId = newJO.Id,
                JOAnalysisId = newAnalysis.Id,
                OptionNumber = 0
            };

            await context.JOCompanyCompensation.AddAsync(joCompanyCompensationA);
            await context.SaveChangesAsync();

            List<JOCompanyCompensationItems> joCmpnyCompensationItemsA = new();
            var compItems = await context.CompensationItems.ToListAsync();

            var basicSalary = candidate.CurrentMonthlyBasicSalary.GetValueOrDefault();
            var monthlyAllowance =
                candidate.MonthlyAllowanceAmount.GetValueOrDefault() +
                candidate.NonMonthlyAllowanceAmount.GetValueOrDefault() +
                candidate.MonthlyNonTaxableAllowanceAmount.GetValueOrDefault();

            /*
            Id	ItemName
            1	Basic Pay
            2	13th Month Pay
            3	Other Guaranteed Bonus/es
            4	Bayanihan Bonus
            5	MRI
            6	Hazard Pay
            7	Allowances
            8	Rice Allowance
            9	Transportation Allowance
            10	Pharmacist Allowance
            11	Profit Share
            12	Sales Incentives
            13	Performance Bonus
            14	TOT Vehicle
             */

            foreach (var item in compItems)
            {
                var compensationItem = item.Id switch
                {
                    1 => new JOCompanyCompensationItems { MonthlyAmount = basicSalary, AnnualAmount = basicSalary * 12m },
                    2 => new JOCompanyCompensationItems { AnnualAmount = basicSalary },
                    3 => new JOCompanyCompensationItems { AnnualAmount = candidate.AnnualGuaranteedBonusAmount.GetValueOrDefault() },
                    7 => new JOCompanyCompensationItems
                    {
                        MonthlyAmount = monthlyAllowance,
                        AnnualAmount = monthlyAllowance * 12m + candidate.AnnualNonTaxableAllowanceAmount.GetValueOrDefault()
                    },
                    11 => new JOCompanyCompensationItems { AnnualAmount = candidate.AnnualProfitSharingAmount.GetValueOrDefault() },
                    12 => new JOCompanyCompensationItems { AnnualAmount = candidate.AnnualIncentiveAmount.GetValueOrDefault() },
                    13 => new JOCompanyCompensationItems { AnnualAmount = candidate.AnnualVariablePayAmount.GetValueOrDefault() },
                    _ => null
                };

                if (compensationItem is not null)
                {
                    compensationItem.ItemId = item.Id;
                    compensationItem.JobOfferId = newJO.Id;
                    compensationItem.JOCmpnyCmpnstnId = joCompanyCompensationA.Id;
                    joCmpnyCompensationItemsA.Add(compensationItem);
                }
            }

            await context.JOCompanyCompensationItems.AddRangeAsync(joCmpnyCompensationItemsA);
            await context.SaveChangesAsync();

            //Options
            List<JOCompanyCompensation> joCompanyCompensationsB = new();
            for (int i = 1; i <= options; i++)
            {
                joCompanyCompensationsB.Add(new JOCompanyCompensation
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdBy,
                    JobOfferId = newJO.Id,
                    JOAnalysisId = newAnalysis.Id,
                    CSGId = candidate.CSGId,
                    OptionNumber = i,
                    CurrentSalary = basicSalary
                });
            }

            await context.JOCompanyCompensation.AddRangeAsync(joCompanyCompensationsB);
            await context.SaveChangesAsync();

            List<JOCompanyCompensationItems> joCmpnyCompensationItemsB = new();
            foreach (var joCompanyCompensation in joCompanyCompensationsB)
            {
                foreach (var item in compItems)
                {
                    joCmpnyCompensationItemsB.Add(new JOCompanyCompensationItems
                    {
                        JobOfferId = newJO.Id,
                        JOCmpnyCmpnstnId = joCompanyCompensation.Id,
                        ItemId = item.Id
                    });
                }
            }

            //JOActionLogs
            JOActionLogs actionLogs = new JOActionLogs
            {
                JobOfferId = newJO.Id,
                RoleId = 1, //TA Partner
                ActionId = 1, //Created
                ActionAt = DateTime.Now,
                ActionBy = createdBy
            };

            //DboxCandidates
            var dboxCandidate = await context.DboxCandidates.FindAsync(candidate.Id);
            dboxCandidate.StatusId = 2;//JO Draft

            await context.JOCompanyCompensationItems.AddRangeAsync(joCmpnyCompensationItemsB);
            await context.JOActionLogs.AddAsync(actionLogs);
            context.DboxCandidates.Update(dboxCandidate);
            await context.SaveChangesAsync();

            return newJO.Id;
        }

        public async Task<int> CreateJobOffer(int candidateId, int createdBy)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            //job offer
            int countJO = await context.JobOffers.CountAsync() + 1;
            var newJO = new JobOffers
            {
                RefNum = $"JO-{DateTime.Now.Year}-{countJO:D5}",
                CandidateId = candidateId,
                StatusId = JOStatus.Application.New,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };
            await context.JobOffers.AddAsync(newJO);
            await context.SaveChangesAsync();

            //workflow
            /*
            var flowStatus = await context.WorkFlowStatus
                .AsNoTracking()
                .OrderBy(jo => jo.DisplayOrder)
                .ToListAsync();

            List<WorkFlow> workFlows = new();
            foreach (var item in flowStatus)
            {
                workFlows.Add(new WorkFlow
                {
                    JobOfferId = newJO.Id,
                    StatusId = item.Id,
                    ActionId = JOStatus.Action.Open
                });
            }

            workFlows[0].ActionId = JOStatus.Action.Done; //Pre-Analysis
            workFlows[1].ActionId = JOStatus.Action.Current; //Company
            workFlows[2].ActionId = JOStatus.Action.Current; //JO Analysis
            workFlows[3].ActionId = JOStatus.Action.Current; //Salary Check
            workFlows[4].ActionId = JOStatus.Action.Next; //Approval

            await context.WorkFlow.AddRangeAsync(workFlows);
            await context.SaveChangesAsync();*/

            return newJO.Id;
        }

        public async Task<int> EmailRequest(Requests entity)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            //email
            //var candidate = await context.Candidates.FindAsync(entity.CandidateId);
            //EmailRequest emailRequest = new EmailRequest
            //{
            //    To = candidate.Email,
            //    Subject = entity.Subject,
            //    Body = entity.Message
            //};

            //await _email.SendAsync(emailRequest);

            //save request
            await context.Requests.AddAsync(entity);
            await context.SaveChangesAsync();

            //job offer
            int countJO = await context.JobOffers.CountAsync() +1;
            var newJO = new JobOffers
            {
                RefNum = $"JO-{DateTime.Now.Year}-{countJO:D5}",
                CandidateId = entity.CandidateId,
                RequestId = entity.Id,
                StatusId = JOStatus.Request.Awaiting,
                CreatedAt = DateTime.Now,
                CreatedBy = entity.CreatedBy
            };
            await context.JobOffers.AddAsync(newJO);
            await context.SaveChangesAsync();

            //workflow
            var flowStatus = await context.WorkFlowStatus
                .AsNoTracking()
                .OrderBy(jo => jo.DisplayOrder)
                .ToListAsync();

            List<WorkFlow> workFlows = new();
            foreach (var item in flowStatus)
            {
                workFlows.Add(new WorkFlow
                {
                    JobOfferId = newJO.Id,
                    StatusId = item.Id,
                    ActionId = JOStatus.Action.Open
                });
            }

            workFlows[0].ActionId = JOStatus.Action.Done;
            workFlows[1].ActionId = JOStatus.Action.Current;
            workFlows[2].ActionId = JOStatus.Action.Current;
            workFlows[3].ActionId = JOStatus.Action.Current;
            workFlows[4].ActionId = JOStatus.Action.Next;

            await context.WorkFlow.AddRangeAsync(workFlows);
            await context.SaveChangesAsync();

            return newJO.Id;
        }

        /*
        public async Task<PagedResult<VwCandidates>> SearchCandidatesAsync(
                int statusId,
                string? candidate,
                int page,
                int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.VwCandidates
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(candidate))
                query = query.Where(jo =>
                    EF.Functions.Like(jo.LastName, $"%{candidate}%") ||
                    EF.Functions.Like(jo.FirstName, $"%{candidate}%"));

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<PagedResult<VwCandidates>> CandidatesForJOCretionAsync(
                string? candidate,
                int page,
                int pageSize)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            int[] validStatus = [
                JOCandidateStatus.Creation,
                JOCandidateStatus.InProgress,
                JOCandidateStatus.Withdrawn];

            var query = context.VwCandidates
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(candidate))
                query = query.Where(jo =>
                    EF.Functions.Like(jo.LastName, $"%{candidate}%") ||
                    EF.Functions.Like(jo.FirstName, $"%{candidate}%"));

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<string> GetCandidateEmail(int id)
        {
            var candidate = await GetCandidate(id);
            return candidate.Email;
        }

        public async Task<int> UpdatePersonalInfo(int id,
            string firstName,
            string lastName,
            string email,
            string contactNumber,
            bool isHrod)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var candidate = await context.Candidates.FindAsync(id);
            candidate.FName = firstName;
            candidate.LName = lastName;
            candidate.Email = email;
            candidate.Contact = contactNumber;

            context.Candidates.Update(candidate);
            return await context.SaveChangesAsync();
        }

        public async Task<VwCandidates> GetCandidate(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<IEnumerable<VwCandidates>> GetAllCandidates()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwCandidates.AsNoTracking().ToListAsync();
        }*/
    }
}
