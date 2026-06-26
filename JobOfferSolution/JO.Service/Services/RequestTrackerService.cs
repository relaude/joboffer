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
            CandidateMSFormRequests newRequest = new CandidateMSFormRequests
            {
                CandidateId = dto.CandidateId,
                RequestSentDate = DateTime.Now,
                DueDate = dto.DueDate,
                ReferenceNumber = await CreateReferenceNumber(),
                StatusId = FormRequestStatus.Awaiting,
                EmailSubject = dto.EmailSubject,
                EmailBody = dto.EmailBody,
                Reminder1SentDate = dto.Reminder1 ? dto.DueDate.AddDays(3) : null,
                Reminder2SentDate = dto.Reminder2 ? dto.DueDate.AddDays(5) : null,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            await context.CandidateMSFormRequests.AddAsync(newRequest);
            await context.SaveChangesAsync();

            return newRequest.Id;
        }

        private async Task<string> CreateReferenceNumber()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var countPlusOne = context.CandidateMSFormRequests.Count() + 1;
            return $"JO-REQ-{DateTime.Now.Year}-{countPlusOne:D5}";
        }
    }
}
