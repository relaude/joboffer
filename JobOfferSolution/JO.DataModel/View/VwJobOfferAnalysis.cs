using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferAnalysis
    {
        public int Id { get; set; }
        public int? CandidateApplicationId { get; set; }
        public int? PackageId { get; set; }
        public int? CandidateId { get; set; }
        public int? RecommendProposalId { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateEmail { get; set; }
        public string? PositionAppliedFor { get; set; }
        public string? Currency { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public decimal? BestProposalSalary { get; set; }
        public string? AnalysisNotes { get; set; }
        public string? PackageName { get; set; }
        public string? ValidationStatusName { get; set; }
        public string? RiskLevelName { get; set; }
        public string? RiskLevelReason { get; set; }
        public decimal? IncreasePercentage { get; set; }
        public decimal? CompaRatio { get; set; }
        public string? MatrixCode { get; set; }
        public string? MatrixName { get; set; }
        public string? JobLevelName { get; set; }
        public string? JobFamilyName { get; set; }
        public string? PositionGrade { get; set; }
        public decimal? BandMinimum { get; set; }
        public decimal? BandMidpoint { get; set; }
        public decimal? BandMaximum { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
    }
}
