using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public EmailTemplateService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwEmailTemplate>> GetVwEmailTemplate()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwEmailTemplate
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<EmailTemplate> GetEmailTemplate(int templateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.EmailTemplate.FindAsync(templateId);
        }

        public async Task<List<EmailTemplateRoles>> GetEmailTemplateRoles(int templateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.EmailTemplateRoles
                .AsNoTracking()
                .Where(jo => jo.EmailTemplateId == templateId)
                .ToListAsync();
        }

        public async Task UpdateEmailTemplate(EmailTemplate emailTemplate, HashSet<int> selectedRecipientRoleIds)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var existingTemplate = await context.EmailTemplate.FindAsync(emailTemplate.Id)
                ?? throw new InvalidOperationException("Email template was not found.");

            existingTemplate.WorkFlowId = emailTemplate.WorkFlowId;
            existingTemplate.EmailSubject = emailTemplate.EmailSubject;
            existingTemplate.EmailMessage = emailTemplate.EmailMessage;
            existingTemplate.OtherRecipient = emailTemplate.OtherRecipient;
            existingTemplate.IsActive = emailTemplate.IsActive;
            existingTemplate.ModifiedBy = emailTemplate.ModifiedBy;
            existingTemplate.ModifiedAt = DateTime.Now;

            var existingRoles = await context.EmailTemplateRoles
                .Where(role => role.EmailTemplateId == emailTemplate.Id)
                .ToListAsync();
            context.EmailTemplateRoles.RemoveRange(existingRoles);
            context.EmailTemplateRoles.AddRange(selectedRecipientRoleIds.Select(roleId => new EmailTemplateRoles
            {
                EmailTemplateId = emailTemplate.Id,
                RoleId = roleId
            }));

            await context.SaveChangesAsync();
        }

        public async Task<int> CreateEmailTemplate(EmailTemplate emailTemplate, HashSet<int> selectedRecipientRoleIds)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            emailTemplate.IsActive = true;
            emailTemplate.CreatedAt = DateTime.Now;

            await context.EmailTemplate.AddAsync(emailTemplate);
            await context.SaveChangesAsync();

            List<EmailTemplateRoles> newTemplateRoles = new();
            foreach (var item in selectedRecipientRoleIds)
            {
                newTemplateRoles.Add(new EmailTemplateRoles { EmailTemplateId = emailTemplate.Id, RoleId = item });
            }

            await context.EmailTemplateRoles.AddRangeAsync(newTemplateRoles);
            await context.SaveChangesAsync();

            return emailTemplate.Id;
        }

        public async Task<List<EmailTemplateRoles>> GetEmailTemplateRoles()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.EmailTemplateRoles.AsNoTracking().ToListAsync();
        }

        public async Task<List<JOWorkFlowStatus>> GetJOWorkFlowStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            return await context.JOWorkFlowStatus
                .AsNoTracking()
                .OrderBy(jo=>jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<JOWorkFlowStatus>> GetFilteredJOWorkFlowStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            List<int?> excludeIds = [1, 2];

            var emailTemplate = await context.EmailTemplate.AsNoTracking().ToListAsync();
            var otherExcludeIds = emailTemplate.Select(jo=> jo.WorkFlowId).ToList();
            excludeIds.AddRange(otherExcludeIds);

            return await context.JOWorkFlowStatus
                .AsNoTracking()
                .Where(jo=> !excludeIds.Contains(jo.Id))
                .OrderBy(jo=>jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<VwJOUserRoles>> GetVwJOUserRoles()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJOUserRoles
                .AsNoTracking()
                .OrderBy(jo=>jo.OrderBy)
                .ToListAsync();
        }
    }
}
