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
    public class DraftAnalysisService : IDraftAnalysisService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public DraftAnalysisService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveAnalysis(
            List<CompensationPackage> compenPackageOptions,
            List<CompensationOptions> compenOptions,
            int templateId,
            int userId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            foreach (var item in compenPackageOptions)
            {
                item.PckgTempId = templateId;
                item.ModifiedBy = userId;
                item.ModifiedAt = DateTime.Now;
            }

            context.CompensationPackage.UpdateRange(compenPackageOptions);
            context.CompensationOptions.UpdateRange(compenOptions);

            return await context.SaveChangesAsync();
        }

        public async Task<List<CompensationDto>> GetCompensationDto()
        {
            List<CompensationDto> compenDto = new();

            await using var context = await _dbContext.CreateDbContextAsync();
            var compenItem = await context.CompensationItem.AsNoTracking().ToListAsync();

            foreach (var item in compenItem)
            {
                compenDto.Add(new CompensationDto
                {
                    Id = item.Id,
                    ItemName = item.ItemName,
                    CurrentMonthly = 0,
                    CurrentAnnual = 0,
                    OptionMonthly = 0,
                    OptionAnnual = 0
                });
            }
            
            return compenDto;
        }

        public async Task<List<CompensationOptions>> GetCompensationOptions(List<int> packageIds)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompensationOptions.AsNoTracking()
                .Where(jo=> packageIds.Contains(jo.PackageId.GetValueOrDefault()))
                .ToListAsync();
        }

        public async Task<List<CompensationPackage>> GetCompensationPackage(int jobOfferId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompensationPackage.AsNoTracking()
                .Where(jo=>jo.JobOfferId == jobOfferId)
                .ToListAsync();
        }

        public async Task<List<CompensationPackage>> GetCompensationPackage()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.CompensationPackage.AsNoTracking().ToListAsync();
        }

        public string ComputeAnnualDiffPay(List<OptionDto> optionDto, 
            List<ComparisonDto> comparisonDto,
            int optionNum)
        {
            decimal propSum = optionDto.Where(jo => jo.OptionNum == optionNum)
                .Sum(jo => jo.MbsAnnualy
                    + jo.Month13Annualy
                    + jo.Month14Annualy
                    + jo.AllowanceAnnualy
                    + jo.PSAnnualy
                    + jo.IncentiveAnnualy
                    + jo.PerformanceAnnualy);

            decimal currSum = comparisonDto.Sum(jo => jo.CurAnnual.GetValueOrDefault());

            decimal diffPay = (propSum - currSum) / currSum;
            decimal truncatedDiffPay = Math.Truncate(diffPay * 100m) / 100m;

            return $"{truncatedDiffPay:F2}%";
        }

        public string ComputeMonthDiffPay(List<OptionDto> optionDto, 
            List<ComparisonDto> comparisonDto,
            int optionNum)
        {
            decimal propSum = optionDto.Where(jo => jo.OptionNum == optionNum)
                .Sum(jo => jo.MbsMonthly + jo.AllowanceMonthly);

            decimal currSum = comparisonDto.Sum(jo => jo.CurMonthy.GetValueOrDefault());

            decimal diffPay = (propSum - currSum) / currSum;
            decimal truncatedDiffPay = Math.Truncate(diffPay * 100m) / 100m;

            return $"{truncatedDiffPay:F2}%";
        }

        public void IncreasePercentage(OptionDto option,
            VwDboxCandidates candidate,
            List<ComparisonDto> comparisonDto)
        {
            // Proposed Salary = Current Salary × (1 + (Increase / 100))
            decimal currentSalary =
                candidate.CurrentMonthlyBasicSalary.GetValueOrDefault();
            
            option.Increase = decimal.Parse(option.IncreaseStr);
            decimal increasePercentage = option.Increase;
            decimal proposedSalary = currentSalary * (1m + (increasePercentage / 100m));

            option.MbsMonthly = proposedSalary;
            option.MbsAnnualy = proposedSalary * 12m;
            option.Month13Annualy = proposedSalary;
            option.Month14Annualy = proposedSalary * 2m;
            option.PSAnnualy = proposedSalary;
            option.PerformanceAnnualy = proposedSalary * 2m;

            comparisonDto[0].OptMonthy = proposedSalary;
            comparisonDto[0].OptAnnual = proposedSalary * 12m;
            comparisonDto[1].OptAnnual = proposedSalary;
            comparisonDto[2].OptAnnual = proposedSalary * 2m;
            comparisonDto[4].OptAnnual = proposedSalary;
            comparisonDto[6].OptAnnual = proposedSalary * 2m;
        }

        public void AddProposalDto(List<OptionDto> optionDto,
            List<VwPckgTempHasItms> tempItems,
            VwDboxCandidates candidate)
        {
            int count = optionDto.Count();
            int lastId = optionDto[count - 1].Id;
            int lastOptNum = optionDto[count - 1].OptionNum;

            decimal monthBasic = candidate.CurrentMonthlyBasicSalary.GetValueOrDefault();
            decimal annualBasic = monthBasic * 12;

            decimal allowMonth = 0;
            decimal allowAnnual = 0;
            if (tempItems.Any())
            {
                var packageItems = tempItems.Where(jo => jo.IsEnabled == true && jo.Analysis == true).ToList();
                foreach (var item in packageItems)
                {
                    allowMonth = allowMonth + item.Monthly.GetValueOrDefault();
                    allowAnnual = allowAnnual + item.Annualy.GetValueOrDefault();
                }
            }

            optionDto.Add(new OptionDto
            {
                Id = lastId + 1,
                OptionNum = lastOptNum + 1,
                Increase = 0,
                Recommend = false,
                MbsMonthly = monthBasic,
                MbsAnnualy = annualBasic,
                Month13Annualy = monthBasic,
                Month14Annualy = monthBasic * 2m,
                AllowanceMonthly = allowMonth,
                AllowanceAnnualy = allowAnnual,
                PSAnnualy = monthBasic,
                PerformanceAnnualy = monthBasic * 2m
            });
        }

        public void RemoveProposalDto(List<OptionDto> optionDto, OptionDto remove)
        {
            int count = optionDto.Count();
            if (count == 1)
            {
                optionDto[0].Id = 1;
                return;
            }

            optionDto.Remove(remove);
            for (int i = 0; i < count - 1; i++)
            {
                optionDto[i].Id = i + 1;
            }
        }

        public void InitProposalDto(List<OptionDto> optionDto, 
            int numProposal,
            VwDboxCandidates candidate)
        {
            decimal monthBasic = candidate.CurrentMonthlyBasicSalary.GetValueOrDefault();
            decimal annualBasic = monthBasic * 12;

            for (int i = 1; i <= numProposal; i++)
            {
                optionDto.Add(new OptionDto
                {
                    Id = i,
                    OptionNum = i,
                    Increase = 0,
                    Recommend = false,
                    MbsMonthly = monthBasic,
                    MbsAnnualy = annualBasic,
                    Month13Annualy = monthBasic,
                    Month14Annualy = monthBasic * 2m,
                    PSAnnualy = monthBasic,
                    PerformanceAnnualy = monthBasic * 2m
                });
            }
        }

        public void FillComparisonDto(List<ComparisonDto> comparisonDto,
            VwDboxCandidates candidate,
            List<OptionDto> optionDto)
        {
            decimal monthAllowance = candidate.MonthlyAllowanceAmount.GetValueOrDefault()
            + candidate.NonMonthlyAllowanceAmount.GetValueOrDefault()
            + candidate.MonthlyNonTaxableAllowanceAmount.GetValueOrDefault();

            decimal annualAllowance = (monthAllowance * 12) + candidate.AnnualNonTaxableAllowanceAmount.GetValueOrDefault();

            comparisonDto.AddRange(
                new ComparisonDto
                {
                    Id = 1,
                    OptionId = 0,
                    Compensation = "Monthly Basic Salary (MBS)",
                    CurMonthy = candidate.CurrentMonthlyBasicSalary,
                    CurAnnual = candidate.CurrentMonthlyBasicSalary * 12,
                    OptMonthy = candidate.CurrentMonthlyBasicSalary,
                    OptAnnual = candidate.CurrentMonthlyBasicSalary * 12
                },

                new ComparisonDto
                {
                    Id = 2,
                    OptionId = 0,
                    Compensation = "13th Month Pay (1.00 x MBS)",
                    CurAnnual = candidate.CurrentMonthlyBasicSalary,
                    OptAnnual = candidate.CurrentMonthlyBasicSalary * 12
                },

                new ComparisonDto
                {
                    Id = 3,
                    Compensation = "Other Guaranteed Bonus/es",
                    CurAnnual = candidate.AnnualGuaranteedBonusAmount
                },

                new ComparisonDto
                {
                    Id = 4,
                    OptionId = 0,
                    Compensation = "Allowance/s",
                    CurMonthy = monthAllowance,
                    CurAnnual = annualAllowance
                },

                new ComparisonDto
                {
                    Id = 5,
                    Compensation = "Profit Share",
                    CurAnnual = candidate.AnnualProfitSharingAmount
                },
                new ComparisonDto
                {
                    Id = 6,
                    Compensation = "Incentives/Commmission",
                    CurAnnual = candidate.AnnualIncentiveAmount
                },
                new ComparisonDto
                {
                    Id = 7,
                    Compensation = "Performance-Based Bonuses",
                    CurAnnual = candidate.AnnualVariablePayAmount
                }
            );
        }

        public async Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int templateId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwPckgTempHasItms
                .AsNoTracking()
                .Where(jo => jo.TempId == templateId)
                .ToListAsync();
        }

        public void OnSelectTemplate(List<VwPckgTempHasItms> tempItems,
            List<ComparisonDto> comparisonDto,
            List<OptionDto> optionDto)
        {
            

            if (tempItems.Any())
            {
                decimal allowMonth = 0;
                decimal allowAnnual = 0;

                //Rice Allowance, Transportation Allowance, Hazard Pay, Pharmacist Allowance
                int?[] allowanceIds = [6, 8, 11, 12];
                var packageItems = tempItems.Where(jo => jo.IsEnabled == true && jo.Analysis == true).ToList();
                
                foreach (var item in packageItems)
                {
                    if (allowanceIds.Contains(item.ItemId))
                    {
                        allowMonth = allowMonth + item.Monthly.GetValueOrDefault();
                        allowAnnual = allowMonth + item.Annualy.GetValueOrDefault();
                    }
                }

                comparisonDto[3].OptMonthy = allowMonth;
                comparisonDto[3].OptAnnual = allowAnnual;

                foreach (var item in optionDto)
                {
                    item.AllowanceMonthly = allowMonth;
                    item.AllowanceAnnualy = allowAnnual;
                }
            }

            if (!tempItems.Any())
            {
                comparisonDto[3].OptMonthy = 0;
                comparisonDto[3].OptAnnual = 0;

                foreach (var item in optionDto)
                {
                    item.AllowanceMonthly = 0;
                    item.AllowanceAnnualy = 0;
                }
            }
        }
    }
}
