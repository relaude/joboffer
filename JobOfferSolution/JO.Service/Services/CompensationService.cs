using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JO.Service.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;

        public CompensationService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SubmitForApproval(JobOffers jobOffer,
            List<JOCompanyCompensation> joCompanyCompensation,
            List<JOCompanyCompensationItems> joCompanyCompensationItems,
            int selectedCmpnyCmpnstnId,
            int userId)
        {
            jobOffer.StatusId = 2;//For Approval
            jobOffer.ModifiedBy = userId;
            jobOffer.ModifiedAt = DateTime.Now;
            jobOffer.Escalate = joCompanyCompensation.Any(jo => jo.Escalate == true);
            jobOffer.OfferRangeId = joCompanyCompensation.Max(jo => jo.OfferRangeId);

            await using var context = await _dbContext.CreateDbContextAsync();
            context.JobOffers.Update(jobOffer);
            await context.SaveChangesAsync();

            await SaveAnalysis(joCompanyCompensation, 
                joCompanyCompensationItems, 
                selectedCmpnyCmpnstnId, 
                userId);

            return jobOffer.Id;
        }

        public async Task<int> SaveAnalysis(List<JOCompanyCompensation> joCompanyCompensation,
            List<JOCompanyCompensationItems> joCompanyCompensationItems,
            int selectedCmpnyCmpnstnId,
            int userId)
        {
            foreach (var joCompensation in joCompanyCompensation)
            {
                joCompensation.CmpnyCmpnstnId = selectedCmpnyCmpnstnId;
                joCompensation.ModifiedBy = userId;
                joCompensation.ModifiedAt = DateTime.Now;
            }

            await using var context = await _dbContext.CreateDbContextAsync();
            context.JOCompanyCompensation.UpdateRange(joCompanyCompensation);
            context.JOCompanyCompensationItems.UpdateRange(joCompanyCompensationItems);

            return await context.SaveChangesAsync();
        }

        public async Task<VwSalaryBands> GetVwSalaryBand(int companyId, int csgId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwSalaryBands.FirstOrDefaultAsync(jo=>jo.CompanyId==companyId && jo.CSGId==csgId);
        }

        public async Task<List<CompenItemCategoryDto>> SetUpCompenItemCategoryDto()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var categories = await context.CompenItemCategory.AsNoTracking().ToListAsync();
            var compensationItems = await context.CompensationItems.AsNoTracking().ToListAsync();

            return categories
                .GroupJoin(
                    compensationItems,
                    category => category.Id,
                    item => item.CategoryId,
                    (category, items) => new CompenItemCategoryDto
                    {
                        Id = category.Id,
                        CategoryName = category.CategoryName,
                        CompensationItemDtos = items.Select(item => new CompensationItemDto
                        {
                            Id = item.Id,
                            CategoryId = item.CategoryId,
                            ItemName = item.ItemName,
                            DisplayOrder = item.DisplayOrder
                        }).ToList()
                    })
                .ToList();
        }

        public async Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOCompanyCompensation
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<JOCompanyCompensationItems>> GetJOCmpnyCompensationItems(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JOCompanyCompensationItems
                .AsNoTracking()
                .Where(jo => jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<CompanyCompensation>> GetCompanyCompensation(int companyId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompanyCompensation
                .AsNoTracking()
                .Where(jo=>jo.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<JobOffers> GetJobOffer(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.JobOffers.FindAsync(jobOfferId);
        }

        public async Task<int> CreateJobOffer(VwDboxCandidates candidate, int options, int createdBy)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            int countJO = await context.JobOffers.CountAsync() + 1;
            var newJO = new JobOffers
            {
                RefNum = $"JO-{DateTime.Now.Year}-{candidate.DboxRefNum}-{countJO:D5}",
                CompanyId = candidate.CompanyId ,
                DivisionId = candidate.DivisionId ,
                DepartmentId = candidate.DepartmentId,
                CandidateId = candidate.Id,
                Options = options,
                StatusId = JOStatus.Application.New,
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy
            };
            await context.JobOffers.AddAsync(newJO);
            await context.SaveChangesAsync();

            //Candidate Current
            JOCompanyCompensation joCompanyCompensationA = new JOCompanyCompensation
            {
                CreatedAt = DateTime.Now,
                CreatedBy = createdBy,
                JobOfferId = newJO.Id,
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

            await context.JOCompanyCompensationItems.AddRangeAsync(joCmpnyCompensationItemsB);
            await context.SaveChangesAsync();

            return newJO.Id;
        }

        public async Task<List<VwCompanyCompensation>> GetVwCompanyCompensation()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompanyCompensation
                .AsNoTracking()
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

        public async Task<int> UpdateCompensationItems(List<VwCompanyCompensationItems> compensationItems, int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var compensation = await context.CompanyCompensation
                .FindAsync(compensationItems.FirstOrDefault().CmpnyCmpnstnId.GetValueOrDefault());

            compensation.ModifiedAt = DateTime.Now;
            compensation.ModifiedBy = userId;

            var updateItems = compensationItems.Select(jo => new CompanyCompensationItems
            {
                Id = jo.Id,
                CmpnyCmpnstnId = jo.CmpnyCmpnstnId,
                ItemId = jo.ItemId,
                MonthlyAmount = jo.MonthlyAmount,
                AnnualAmount = jo.AnnualAmount,
                IsAnalysis = jo.IsAnalysis,
                IsEditable = jo.IsEditable
            }).ToList();

            context.CompanyCompensation.Update(compensation);
            context.CompanyCompensationItems.UpdateRange(updateItems);
            return await context.SaveChangesAsync();
        }
    }
}
