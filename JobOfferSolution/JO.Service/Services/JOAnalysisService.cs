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

        public async Task<List<VwCompensationBenefits>> GetCompensationBenefits(int packageId)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwCompensationBenefits.AsNoTracking()
                .Where(jo=>jo.PackageId==packageId)
                .OrderBy(jo=>jo.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<SalaryBandStatus>> GetValidationStatus()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.SalaryBandStatus.AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveProposal(List<Proposal> proposals)
        {
            int jobOfferId = proposals.FirstOrDefault().JobOfferId.GetValueOrDefault();

            await using var context = await _dbContext.CreateDbContextAsync();

            //joboffer
            var jobOffer = await context.JobOffers.FindAsync(jobOfferId);
            jobOffer.StatusId = JOStatus.Application.ProposalCreated;

            //workflow
            bool hasEscalation = proposals.Any(jo => jo.Escalate == true);
            var joWorkFlow = await context.WorkFlow
                .Where(jo => jo.JobOfferId == jobOfferId)
                .Take(7)
                .ToListAsync();

            joWorkFlow[2].ActionId = JOStatus.Action.Done;
            joWorkFlow[3].ActionId = JOStatus.Action.Done;
            joWorkFlow[4].ActionId = JOStatus.Action.Current;

            joWorkFlow[5].ActionId = hasEscalation ? JOStatus.Action.Next : JOStatus.Action.Open;
            joWorkFlow[6].ActionId = hasEscalation ? JOStatus.Action.Open : JOStatus.Action.Next;

            //updating...
            context.JobOffers.Update(jobOffer);
            context.WorkFlow.UpdateRange(joWorkFlow);
            await context.Proposal.AddRangeAsync(proposals);

            return await context.SaveChangesAsync();
        }

        public List<Proposal> ReComputeAnalysis(
            List<Proposal> proposals,
            VwSalaryMatrixBand matrixBand,
            List<VwCompensationBenefits> compBen)
        {
            decimal packages = ComputePackagesAnnualAmount(compBen);

            foreach (var proposal in proposals)
            {
                decimal propose = proposal.ProposeSalary.GetValueOrDefault();
                decimal current = proposal.CurrentSalary.GetValueOrDefault();
                decimal midpoint = matrixBand.BandMidpoint.GetValueOrDefault();

                decimal inrease = ComputeIncreasePercentage(propose, current);
                decimal compaRatio = Math.Round((propose / midpoint), 2);
                decimal annualSalary = (propose * 12) + packages;
                int statusId = SetValidationStatusId(propose, matrixBand);

                proposal.Increase = inrease;
                proposal.CompaRatio = compaRatio;
                proposal.Annual = annualSalary;
                proposal.StatusId = statusId;
            }

            return proposals;
        }

        public List<Proposal> InitializeProposal(
            int jobOfferId,
            int salaryBandId,
            decimal current,
            VwSalaryMatrixBand matrixBand,
            List<VwCompensationBenefits> compBen)
        {
            List<Proposal> proposals = new();

            decimal midpoint = matrixBand.BandMidpoint.GetValueOrDefault();
            decimal packages = ComputePackagesAnnualAmount(compBen);
            
            for (int i = 1; i <= 3; i++)
            {
                decimal increase = i * 10;
                decimal propose = ComputeProposedSalary(current, increase);
                decimal annual = (propose * 12) + packages;
                decimal compaRatio = Math.Round((propose / midpoint), 2);
                int statusId = SetValidationStatusId(propose, matrixBand);

                proposals.Add(new Proposal {
                    JobOfferId = jobOfferId,
                    SalaryBandId = salaryBandId,
                    OptionNum = i,
                    CurrentSalary = current,
                    ProposeSalary = propose,
                    CompaRatio = compaRatio,
                    Increase = increase,
                    Annual = annual,
                    StatusId = statusId,
                    Recommend = i==1
                });
            }

            return proposals;
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

        private int SetValidationStatusId(decimal proposedSalary, VwSalaryMatrixBand matrixBand)
        {
            if (!matrixBand.BandMinimum.HasValue ||
                !matrixBand.BandMidpoint.HasValue ||
                !matrixBand.BandMaximum.HasValue)
                return 0;

            var minimum = matrixBand.BandMinimum.Value;
            var midpoint = matrixBand.BandMidpoint.Value;
            var maximum = matrixBand.BandMaximum.Value;

            if (proposedSalary < minimum)
                return JOStatus.SalaryBand.Lower;

            if (proposedSalary == midpoint)
                return JOStatus.SalaryBand.Midpoint;

            if (proposedSalary > maximum)
                return JOStatus.SalaryBand.Exceed;

            if (proposedSalary > midpoint)
                return JOStatus.SalaryBand.Upper;

            return JOStatus.SalaryBand.Within;
        }

        public async Task<int> LegalEntitySetup(LegalEntities legal)
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var jobOffer = await context.JobOffers.FindAsync(legal.JobOfferId);

            legal.CandidateId = jobOffer.CandidateId;
            await context.LegalEntities.AddAsync(legal);
            await context.SaveChangesAsync();

            jobOffer.LegalId = legal.Id;
            jobOffer.StatusId = JOStatus.Application.MatrixSelected;

            var workFlow = await context.WorkFlow
                .Where(jo => jo.JobOfferId == legal.JobOfferId)
                .Take(5)
                .ToListAsync();

            workFlow[1].ActionId = JOStatus.Action.Done;
            workFlow[2].ActionId = JOStatus.Action.Current;
            workFlow[3].ActionId = JOStatus.Action.Current;
            workFlow[4].ActionId = JOStatus.Action.Next;
            
            context.JobOffers.Update(jobOffer);
            context.WorkFlow.UpdateRange(workFlow);

            return await context.SaveChangesAsync();
        }
    }
}
