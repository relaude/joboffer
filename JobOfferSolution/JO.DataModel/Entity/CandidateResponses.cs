using System;
using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CandidateResponses
    {
        [Key] public int Id { get; set; }
        public int? CandidateResponseId { get; set; }
        public DateTime? ResponseStartedAt { get; set; }
        public DateTime? ResponseCompletedAt { get; set; }
        public string? EmailAddress { get; set; }
        public string? RespondentName { get; set; }
        public string? HasDataPrivacyConsent { get; set; }
        public string? CandidateFullName { get; set; }
        public DateTime? FormCompletedDate { get; set; }
        public string? PositionAppliedFor { get; set; }
        public string? UnilabDivision { get; set; }
        public decimal? ExpectedMonthlyBasicSalary { get; set; }
        public string? Age { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? RelevantExperience { get; set; }
        public string? CurrentEmployerName { get; set; }
        public string? LastEmployerIndustry { get; set; }
        public string? LastPositionHeld { get; set; }
        public decimal? CurrentMonthlyBasicSalary { get; set; }
        public string? GuaranteedMonthsPay { get; set; }
        public string? AnnualGuaranteedBonusDescription { get; set; }
        public decimal? AnnualGuaranteedBonusAmount { get; set; }
        public string? MonthlyAllowanceDescription { get; set; }
        public decimal? MonthlyAllowanceAmount { get; set; }
        public string? NonMonthlyAllowanceDescription { get; set; }
        public decimal? NonMonthlyAllowanceAmount { get; set; }
        public string? MonthlyNonTaxableAllowanceDescription { get; set; }
        public decimal? MonthlyNonTaxableAllowanceAmount { get; set; }
        public string? AnnualNonTaxableAllowanceDescription { get; set; }
        public decimal? AnnualNonTaxableAllowanceAmount { get; set; }
        public decimal? AnnualProfitSharingAmount { get; set; }
        public string? AnnualIncentiveDescription { get; set; }
        public decimal? AnnualIncentiveAmount { get; set; }
        public string? AnnualVariablePayDescription { get; set; }
        public decimal? AnnualVariablePayAmount { get; set; }
        public string? EmployeeHmoBenefitLimit { get; set; }
        public string? DependentHmoBenefitLimit { get; set; }
        public string? DentalBenefit { get; set; }
        public string? MedicineReimbursementBenefit { get; set; }
        public string? OpticalBenefit { get; set; }
        public string? OtherHealthBenefits { get; set; }
        public string? VacationLeaveBenefit { get; set; }
        public string? SickLeaveBenefit { get; set; }
        public string? OtherLeaveBenefits { get; set; }
        public string? LifeInsuranceBenefit { get; set; }
        public string? OtherBenefits { get; set; }
        public string? VehicleBenefit { get; set; }
        public string? MobilePhoneBenefit { get; set; }
        public string? DboxCandidateId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
