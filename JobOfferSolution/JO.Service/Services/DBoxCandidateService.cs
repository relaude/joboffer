using JO.Persistence.DataAccess;
using JO.Service.Services.Contracts;
using JO.DataModel.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using JO.DataModel.View;

namespace JO.Service.Services
{
    public class DBoxCandidateService : IDBoxCandidateService
    {
        private readonly IDbContextFactory<JobOfferDbContext> _dbContext;
        public DBoxCandidateService(IDbContextFactory<JobOfferDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VwDboxCandidates>> GetVwDboxCandidates()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            return await context.VwDboxCandidates.AsNoTracking()
                .OrderByDescending(candidate => candidate.Id).ToListAsync();
        }

        public async Task MergeCandidateAndResponse()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var rawCandidates = await context.DboxCandidatesRawData
                .Where(candidate => candidate.IsCopied == false || candidate.IsCopied == null)
                .ToListAsync();

            if (rawCandidates.Count == 0)
                return;

            var existingReferenceNumbers = await context.DboxCandidates.AsNoTracking()
                .Select(candidate => candidate.DboxRefNum).ToListAsync();
            var candidateReferenceNumbers = new HashSet<string?>(existingReferenceNumbers);

            var companies = await context.Companies.AsNoTracking().ToListAsync();
            var divisions = await context.Divisions.AsNoTracking().ToListAsync();
            var companySalaryGrades = await context.VwCompanySalaryGrades.AsNoTracking().ToListAsync();
            var candidateResponses = await context.CandidateResponses.AsNoTracking().ToListAsync();

            foreach (var rawCandidate in rawCandidates)
            {
                // Include records queued in this batch as well as records already saved.
                if (!candidateReferenceNumbers.Add(rawCandidate.CandidateId))
                    continue;

                var responseId = string.IsNullOrWhiteSpace(rawCandidate.CandidateId)
                    ? (int?)null
                    : candidateResponses.FirstOrDefault(response =>
                        response.DboxCandidateId == rawCandidate.CandidateId)?.Id;
                var company = string.IsNullOrWhiteSpace(rawCandidate.Company)
                    ? null
                    : companies.FirstOrDefault(item => item.CompanyCode == rawCandidate.Company);
                var division = string.IsNullOrWhiteSpace(rawCandidate.Division)
                    ? null
                    : divisions.FirstOrDefault(item => item.DivisionCode == rawCandidate.Division);
                var companyId = company?.Id;
                var csgId = companyId is null || string.IsNullOrWhiteSpace(rawCandidate.JobLevel)
                    ? (int?)null
                    : companySalaryGrades.FirstOrDefault(grade =>
                        grade.CompanyId == companyId && grade.GradeName == rawCandidate.JobLevel)?.Id;

                context.DboxCandidates.Add(new DboxCandidates
                {
                    DboxId = rawCandidate.Id,
                    ResponseId = responseId,
                    CompanyId = companyId,
                    DivisionId = division?.Id,
                    Company = company?.CompanyName,
                    Division = division?.DivisionName,
                    Department = rawCandidate.Department,
                    CSGId = csgId,
                    DboxRefNum = rawCandidate.CandidateId,
                    CandidateName = rawCandidate.CandidateName,
                    CostCenter = rawCandidate.CostCenter,
                    JobLevel = rawCandidate.JobLevel,
                    JobPosition = rawCandidate.JobPosition,
                    EmailAddress = rawCandidate.EmailAddress,
                    ContactNumber = rawCandidate.ContactNumber,
                    StatusId = 1 //For Creation
                });

                rawCandidate.IsCopied = true;
            }

            // Persist inserts and copied flags together so a failed save can be retried.
            await context.SaveChangesAsync();
        }

        public async Task MergeCandidateRawResponses()
        {
            await using var context = await _dbContext.CreateDbContextAsync();
            var rawResponses = await context.CandidateResponseRawData
                .Where(response => (response.InvalidResponse == null || response.InvalidResponse == false)
                    && (response.ErrorMessage == null || response.ErrorMessage == "")
                    && (response.IsCopied == null || response.IsCopied == false))
                .ToListAsync();

            var existingCandidateIds = await context.CandidateResponses.AsNoTracking()
                .Select(response => response.DboxCandidateId).ToListAsync();
            var responseCandidateIds = new HashSet<string?>(existingCandidateIds);

            foreach (var raw in rawResponses)
            {
                // Include responses queued in this batch as well as records already saved.
                if (!responseCandidateIds.Add(raw.CandidateDBoxID))
                    continue;

                context.CandidateResponses.Add(new CandidateResponses
                {
                    CandidateResponseId = raw.Id,
                    DboxCandidateId = raw.CandidateDBoxID,
                    ResponseStartedAt = ParseResponseDate(raw.ResponseStartedAt),
                    ResponseCompletedAt = ParseResponseDate(raw.ResponseCompletedAt),
                    FormCompletedDate = ParseResponseDate(raw.FormCompletedDate),
                    EmailAddress = raw.EmailAddress,
                    RespondentName = raw.RespondentName,
                    HasDataPrivacyConsent = raw.HasDataPrivacyConsent,
                    CandidateFullName = raw.CandidateFullName,
                    PositionAppliedFor = raw.PositionAppliedFor,
                    UnilabDivision = raw.UnilabDivision,
                    ExpectedMonthlyBasicSalary = ParseResponseAmount(raw.ExpectedMonthlyBasicSalary),
                    Age = raw.Age,
                    EmploymentStatus = raw.EmploymentStatus,
                    RelevantExperience = raw.RelevantExperience,
                    CurrentEmployerName = raw.CurrentEmployerName,
                    LastEmployerIndustry = raw.LastEmployerIndustry,
                    LastPositionHeld = raw.LastPositionHeld,
                    CurrentMonthlyBasicSalary = ParseResponseAmount(raw.CurrentMonthlyBasicSalary),
                    GuaranteedMonthsPay = raw.GuaranteedMonthsPay,
                    AnnualGuaranteedBonusDescription = raw.AnnualGuaranteedBonusDescription,
                    AnnualGuaranteedBonusAmount = ParseResponseAmount(raw.AnnualGuaranteedBonusAmount),
                    MonthlyAllowanceDescription = raw.MonthlyAllowanceDescription,
                    MonthlyAllowanceAmount = ParseResponseAmount(raw.MonthlyAllowanceAmount),
                    NonMonthlyAllowanceDescription = raw.NonMonthlyAllowanceDescription,
                    NonMonthlyAllowanceAmount = ParseResponseAmount(raw.NonMonthlyAllowanceAmount),
                    MonthlyNonTaxableAllowanceDescription = raw.MonthlyNonTaxableAllowanceDescription,
                    MonthlyNonTaxableAllowanceAmount = ParseResponseAmount(raw.MonthlyNonTaxableAllowanceAmount),
                    AnnualNonTaxableAllowanceDescription = raw.AnnualNonTaxableAllowanceDescription,
                    AnnualNonTaxableAllowanceAmount = ParseResponseAmount(raw.AnnualNonTaxableAllowanceAmount),
                    AnnualProfitSharingAmount = ParseResponseAmount(raw.AnnualProfitSharingAmount),
                    AnnualIncentiveDescription = raw.AnnualIncentiveDescription,
                    AnnualIncentiveAmount = ParseResponseAmount(raw.AnnualIncentiveAmount),
                    AnnualVariablePayDescription = raw.AnnualVariablePayDescription,
                    AnnualVariablePayAmount = ParseResponseAmount(raw.AnnualVariablePayAmount),
                    EmployeeHmoBenefitLimit = raw.EmployeeHmoBenefitLimit,
                    DependentHmoBenefitLimit = raw.DependentHmoBenefitLimit,
                    DentalBenefit = raw.DentalBenefit,
                    MedicineReimbursementBenefit = raw.MedicineReimbursementBenefit,
                    OpticalBenefit = raw.OpticalBenefit,
                    OtherHealthBenefits = raw.OtherHealthBenefits,
                    VacationLeaveBenefit = raw.VacationLeaveBenefit,
                    SickLeaveBenefit = raw.SickLeaveBenefit,
                    OtherLeaveBenefits = raw.OtherLeaveBenefits,
                    LifeInsuranceBenefit = raw.LifeInsuranceBenefit,
                    OtherBenefits = raw.OtherBenefits,
                    VehicleBenefit = raw.VehicleBenefit,
                    MobilePhoneBenefit = raw.MobilePhoneBenefit,
                    CreatedAt = raw.CreatedAt,
                    CreatedBy = raw.CreatedBy
                });

                raw.IsCopied = true;
            }

            // A parsing or save failure leaves the batch uncopied in the database.
            if (rawResponses.Count > 0)
                await context.SaveChangesAsync();
        }

        private static decimal? ParseResponseAmount(string? value) =>
            string.IsNullOrWhiteSpace(value)
                ? null
                : decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);

        private static DateTime? ParseResponseDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var date)
                || DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out date))
                return date;

            throw new FormatException($"Invalid response date: '{value}'.");
        }
    }
}
