using JO.DataModel.DTOs;
using JO.DataModel.Entity;
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
    public class RequestTrackerService : IRequestTrackerService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IEmailService _email;
        public RequestTrackerService(IDbContextFactory<JobOfferDbContext> dbContext,
            IEmailService email)
        {
            _dbContext = dbContext;
            _email = email;
        }

        public async Task<int> CreateAndEmailMsFormRequest(CandidateMSFormRequestDto dto)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            //candidate
            //var candidate = await context.Candidates.FindAsync(dto.CandidateId);

            //send email
            //EmailRequest emailRequest = new EmailRequest
            //{
            //    To = dto.CandidateEmail,
            //    Subject = dto.EmailSubject,
            //    Body = dto.EmailBody
            //};

            //await _email.SendAsync(emailRequest);

            //save request
            Requests newRequest = new Requests
            {
                CandidateId = dto.CandidateId,
                SentAt = DateTime.Now,
                DueAt = dto.DueDate,
                //ReferenceNumber = await CreateReferenceNumber(),
                //StatusId = JOStatus.FormRequestStatus.Awaiting,
                Subject = dto.EmailSubject,
                Message = dto.EmailBody,
                Reminder1 = dto.Reminder1 ? dto.DueDate.AddDays(3) : null,
                Reminder2 = dto.Reminder2 ? dto.DueDate.AddDays(5) : null,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            await context.Requests.AddAsync(newRequest);
            await context.SaveChangesAsync();

            //init workflow
            var workFlow = await context.WorkFlowStatus
                .AsNoTracking()
                .OrderBy(jo=> jo.DisplayOrder)
                .ToListAsync();

            List<JobOfferWorkFlow> joWorkFlows = new();
            
            foreach (var item in workFlow)
            {
                joWorkFlows.Add(new JobOfferWorkFlow
                {
                    CandidateMSFormRequestId = newRequest.Id,
                    DisplayOrder = item.DisplayOrder,
                    WorkFlowStatusId = item.Id,
                    WorkFlowActionId = item.DisplayOrder == 2 ? JOStatus.Action.Next
                        : (item.DisplayOrder == 1 ? JOStatus.Action.Current 
                            : JOStatus.Action.Open)
                }); 
            }

            await context.JobOfferWorkFlow.AddRangeAsync(joWorkFlows);
            await context.SaveChangesAsync();

            return newRequest.Id;
        }

        private async Task<string> CreateReferenceNumber()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var countPlusOne = context.Requests.Count() + 1;
            return $"JO-REQ-{DateTime.Now.Year}-{countPlusOne:D5}";
        }
    }
}
