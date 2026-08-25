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
    public class JOLetterService : IJOLetterService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        private readonly IUtilitiesService _UtilitiesService;

        public JOLetterService(IDbContextFactory<JobOfferDbContext> dbContext, IUtilitiesService UtilitiesService)
        {
            _dbContext = dbContext;
            _UtilitiesService = UtilitiesService;
        }

        public async Task<JobOffers> GetJobOffer(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(jobOfferId);
        }

        public async Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJODboxCandidates.FirstOrDefaultAsync(jo=> jo.Id==jobOfferId);
        }

        public async Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.FirstOrDefaultAsync(jo => jo.Id == candidateId);
        }

        public async Task<CompanyCompensation> GetCompanyCompensation(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompanyCompensation.FindAsync(compensationId);
        }

        public async Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOCompanyCompensation
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<VwCompanyCompensationItems>> GetVwCompanyCompensationItems(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompanyCompensationItems
                .AsNoTracking()
                .Where(jo => jo.CmpnyCmpnstnId == compensationId)
                .ToListAsync();
        }

        public async Task<List<JOItemLetter>> GetJOItemLetter(int compensationId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            return await context.JOItemLetter
                .AsNoTracking()
                .Where(letter =>
                    letter.ItemId == 0 ||
                    context.CompanyCompensationItems.Any(item =>
                        item.CmpnyCmpnstnId == compensationId &&
                        item.ItemId == letter.ItemId))
                .OrderBy(letter => letter.DisplayOrder)
                .ToListAsync();
        }

        public void UpdateItemLetterPlaceHolder(List<JOItemLetter> joItemLetter,
            VwDboxCandidates candidate,
            decimal proposedSalary)
        {
            string company = candidate.Company ?? string.Empty;
            string position = candidate.JobPosition ?? string.Empty;
            string division = candidate.Division ?? string.Empty;
            string basicPay = _UtilitiesService.ToPeso(proposedSalary);

            (string PlaceHolder, string Value)[] replacements =
            {
                ("[COMPANY]", company),
                ("[POSITION]", position),
                ("[DIVISION]", division),
                ("[BASICPAY]", basicPay)
            };

            foreach (var itemLetter in joItemLetter)
            {
                if (string.IsNullOrEmpty(itemLetter.MessageBody))
                {
                    continue;
                }

                foreach (var replacement in replacements)
                {
                    itemLetter.MessageBody = itemLetter.MessageBody.Replace(
                        replacement.PlaceHolder,
                        replacement.Value,
                        StringComparison.OrdinalIgnoreCase);
                }
            }
        }
    }
}
