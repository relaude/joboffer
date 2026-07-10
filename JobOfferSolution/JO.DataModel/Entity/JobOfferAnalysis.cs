using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferAnalysis
    {
        [Key] public int Id { get; set; }
        public int? CandidateApplicationId { get; set; }
        public int? PackageId { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public decimal? BestProposalSalary { get; set; }
        public int? ValidationStatusId { get; set; }
        public int? RecommendProposalId { get; set; }
        public int? SalaryMatrixBandId { get; set; }
        public string? AnalysisNotes { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
