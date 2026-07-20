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
                    ActionId = item.DisplayOrder == 2 ? JOStatus.Action.Next 
                        : (item.DisplayOrder == 1 ? JOStatus.Action.Current 
                            : JOStatus.Action.Open)
                });
            }
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
