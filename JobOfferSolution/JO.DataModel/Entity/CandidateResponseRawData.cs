using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JO.DataModel.Entity
{
    public class CandidateResponseRawData
    {
        [Key]
        public int Id { get; set; }
        public string? CandidateResponseId { get; set; }
        public string? ResponseStartedAt { get; set; }
        public string? ResponseCompletedAt { get; set; }
        public string? EmailAddress { get; set; }
        public string? RespondentName { get; set; }
        public string? HasDataPrivacyConsent { get; set; }
        public string? CandidateFullName { get; set; }
        public string? FormCompletedDate { get; set; }
        public string? PositionAppliedFor { get; set; }
        public string? UnilabDivision { get; set; }
        public string? ExpectedMonthlyBasicSalary { get; set; }
        public string? Age { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? RelevantExperience { get; set; }
        public string? CurrentEmployerName { get; set; }
        public string? LastEmployerIndustry { get; set; }
        public string? LastPositionHeld { get; set; }
        public string? CurrentMonthlyBasicSalary { get; set; }
        public string? GuaranteedMonthsPay { get; set; }
        public string? AnnualGuaranteedBonusDescription { get; set; }
        public string? AnnualGuaranteedBonusAmount { get; set; }
        public string? MonthlyAllowanceDescription { get; set; }
        public string? MonthlyAllowanceAmount { get; set; }
        public string? NonMonthlyAllowanceDescription { get; set; }
        public string? NonMonthlyAllowanceAmount { get; set; }
        public string? MonthlyNonTaxableAllowanceDescription { get; set; }
        public string? MonthlyNonTaxableAllowanceAmount { get; set; }
        public string? AnnualNonTaxableAllowanceDescription { get; set; }
        public string? AnnualNonTaxableAllowanceAmount { get; set; }
        public string? AnnualProfitSharingAmount { get; set; }
        public string? AnnualIncentiveDescription { get; set; }
        public string? AnnualIncentiveAmount { get; set; }
        public string? AnnualVariablePayDescription { get; set; }
        public string? AnnualVariablePayAmount { get; set; }
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
        
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public bool? InvalidResponse { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
