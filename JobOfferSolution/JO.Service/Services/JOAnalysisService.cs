using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class JOAnalysisService : IJOAnalysisService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public JOAnalysisService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwJobOfferProposal>> GetJobOfferProposal(int analysisId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferProposal.AsNoTracking()
                .Where(jo => jo.JobOfferAnalysisId == analysisId)
                .ToListAsync();
        }

        public async Task<List<VwJobOfferAnalysis>> GetJobOfferAnalysis()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferAnalysis.ToListAsync() ?? new();
        }

        public async Task<VwJobOfferAnalysis> GetJobOfferAnalysis(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwJobOfferAnalysis.FirstOrDefaultAsync(jo=>jo.Id==id) ?? new();
        }

        public async Task<List<VwCompensationBenefits>> GetCompensationBenefits(int packageId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompensationBenefits.AsNoTracking()
                .Where(jo=>jo.PackageId==packageId)
                .OrderBy(jo=>jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<ValidationStatus>> GetValidationStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.ValidationStatus.AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveProposalsAndAnalysis(
            List<JobOfferProposal> proposals,
            int selectedOptionNumber,
            int selectedPackageId,
            decimal expectedSalary,
            string analysisNotes)
        {
            int totalSaves = 0;

            if (proposals is null || !proposals.Any())
                return 0;

            var recommended = proposals.FirstOrDefault(jo =>
                jo.OptionNumber.GetValueOrDefault() == selectedOptionNumber);

            if (recommended is null)
                return 0;

            int applicationId = recommended.CandidateApplicationId.GetValueOrDefault();
            int matrixBandId = recommended.SalaryMatrixBandId.GetValueOrDefault();

            if (applicationId == 0 || matrixBandId == 0)
                return 0;

            await using var context = await _dbContext.CreateDbContextAsync();

            bool existingProposal = await context.JobOfferProposal.AnyAsync(jo =>
                jo.CandidateApplicationId == applicationId &&
                jo.SalaryMatrixBandId == matrixBandId);

            if (existingProposal)
                return 0;

            var analysis = new JobOfferAnalysis
            {
                CandidateApplicationId = applicationId,
                RecommendProposalId = recommended.Id,
                SalaryMatrixBandId = recommended.SalaryMatrixBandId,
                PackageId = selectedPackageId,
                ExpectedSalary = expectedSalary,
                BestProposalSalary = recommended.ProposedSalary.GetValueOrDefault(),
                ValidationStatusId = recommended.ValidationStatusId.GetValueOrDefault(),
                AnalysisNotes = analysisNotes,
                CreatedBy = recommended.CreatedBy.GetValueOrDefault(),
                CreatedAt = DateTime.Now
            };

            await context.JobOfferAnalysis.AddAsync(analysis);
            totalSaves += await context.SaveChangesAsync();

            foreach (var proposal in proposals)
                proposal.JobOfferAnalysisId = analysis.Id;

            await context.JobOfferProposal.AddRangeAsync(proposals);
            totalSaves += await context.SaveChangesAsync();

            var recommended2 = proposals.FirstOrDefault(jo =>
                jo.OptionNumber.GetValueOrDefault() == selectedOptionNumber);

            analysis.RecommendProposalId = recommended2.Id;
            context.JobOfferAnalysis.Update(analysis);
            totalSaves += await context.SaveChangesAsync();

            return analysis.Id;
        }

        public async Task<List<JobOfferProposal>> ReComputeProposalAnalysis(List<JobOfferProposal> proposals, List<VwCompensationBenefits> compensationItems)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var matrixBand = await context.SalaryMatrixBand.FindAsync(proposals.FirstOrDefault().SalaryMatrixBandId.Value);
            decimal packagesAnnualAmount = ComputePackagesAnnualAmount(compensationItems);

            foreach (var proposal in proposals)
            {
                decimal increasePercentage = ComputeIncreasePercentage(proposal.ProposedSalary.Value, proposal.CurrentSalary.Value);
                decimal compaRatio = Math.Round((proposal.ProposedSalary.Value / proposal.SalaryMidpoint.Value), 2);
                decimal annualSalary = (proposal.ProposedSalary.Value * 12) + packagesAnnualAmount;
                int statusId = SetValidationStatusId(proposal.ProposedSalary.Value, matrixBand);

                proposal.IncreasePercentage = increasePercentage;
                proposal.CompaRatio = compaRatio;
                proposal.AnnualSalary = annualSalary;
                proposal.ValidationStatusId = statusId;
            }

            return proposals;
        }

        public async Task<List<JobOfferProposal>> InitializeProposal(
            int applicationId,
            int salaryMatrixId,
            int salaryMatrixBandId,
            decimal currentSalary,
            int createdBy,
            List<VwCompensationBenefits> compensationItems)
        {

            List<JobOfferProposal> proposalList = new();

            await using var context = await _dbContext.CreateDbContextAsync();
            var matrixBand = await context.SalaryMatrixBand.FindAsync(salaryMatrixBandId);
            decimal packagesAnnualAmount = ComputePackagesAnnualAmount(compensationItems);

            for (int i = 1; i <= 3; i++)
            {
                decimal increasePercentage = i * 10;

                decimal proposedSalary = ComputeProposedSalary(currentSalary, increasePercentage);

                //Compa-Ratio = Proposed Salary / Salary Midpoint
                decimal compaRatio = Math.Round((proposedSalary / matrixBand.BandMidpoint.Value), 2);

                decimal annualSalary = (proposedSalary * 12) + packagesAnnualAmount;

                int statusId = SetValidationStatusId(proposedSalary, matrixBand);
                string justification = SetJustificationNotes(statusId);

                proposalList.Add(new JobOfferProposal
                {
                    CandidateApplicationId = applicationId,
                    SalaryMatrixBandId = salaryMatrixId,
                    OptionNumber = i,
                    CurrentSalary = currentSalary,
                    ProposedSalary = proposedSalary,
                    SalaryMidpoint = matrixBand.BandMidpoint,
                    CompaRatio = compaRatio,
                    IncreasePercentage = increasePercentage,
                    AnnualSalary = annualSalary,
                    ValidationStatusId = statusId,
                    Justification = justification,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now
                });
            }

            return proposalList;
        }

        private decimal ComputePackagesAnnualAmount(List<VwCompensationBenefits> compensationItems)
        {
            return compensationItems
                .Where(item => item.Amount > 0)
                .Sum(item => item.Amount.GetValueOrDefault() * item.Multiplier.GetValueOrDefault());
        }

        private decimal ComputeIncreasePercentage(decimal proposedSalary, decimal currentSalary)
        {
            //Increase % = ((Proposed Salary - Current Salary) / Current Salary) × 100
            decimal percentage = ((proposedSalary - currentSalary) / currentSalary) * 100;
            return Math.Round(percentage, 2);
        }

        private decimal ComputeProposedSalary(decimal currentSalary, decimal increasePercentage)
        {
            //Proposed Salary = Current Salary × (1 + (Increase % / 100))
            decimal salary = currentSalary * (1 + (increasePercentage/100));
            return Math.Round(salary, 2);
        }

        private string SetJustificationNotes(int statusId)
        {
            return statusId switch
            {
                JOValidationStatus.Within => "Conservative offer aligned with lower market range.",
                JOValidationStatus.Midpoint => "Recommended offer matching candidate expectation and salary midpoint.",
                JOValidationStatus.Upper => "Competitive offer for faster acceptance while remaining below maximum.",
                JOValidationStatus.Lower => "Proposed salary is below the salary band minimum.",
                JOValidationStatus.Exceed => "Proposed salary exceeds the salary band maximum.",
                _ => string.Empty
            };
        }

        private int SetValidationStatusId(decimal proposedSalary, SalaryMatrixBand matrixBand)
        {
            if (!matrixBand.BandMinimum.HasValue ||
                !matrixBand.BandMidpoint.HasValue ||
                !matrixBand.BandMaximum.HasValue)
                return 0;

            var minimum = matrixBand.BandMinimum.Value;
            var midpoint = matrixBand.BandMidpoint.Value;
            var maximum = matrixBand.BandMaximum.Value;

            if (proposedSalary < minimum)
                return JOValidationStatus.Lower;

            if (proposedSalary == midpoint)
                return JOValidationStatus.Midpoint;

            if (proposedSalary > maximum)
                return JOValidationStatus.Exceed;

            if (proposedSalary > midpoint)
                return JOValidationStatus.Upper;

            return JOValidationStatus.Within;
        }

        public async Task<List<VwCandidateApplications>> GetCandidateApplications()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateApplications
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<VwCandidateApplications> GetCandidateApplication(int id)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCandidateApplications.FirstOrDefaultAsync(jo => jo.Id == id);
        }

        public async Task<int> LegalEntitySetup(CandidateApplications entity)
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            entity.ReferenceNumber = await CreateReferenceNumber();
            entity.CreatedAt = DateTime.Now;
            await context.CandidateApplications.AddAsync(entity);

            await context.SaveChangesAsync();
            return entity.Id;
        }

        private async Task<string> CreateReferenceNumber()
        {
            await using var context = await _dbContext.CreateDbContextAsync();

            var countPlusOne = context.CandidateApplications.Count() + 1;
            return $"JO-APL-{DateTime.Now.Year}-{countPlusOne:D5}";
        }
    }
}
