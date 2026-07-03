using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferProposal
    {
        [Key] public int Id { get; set; }
        public int? CandidateApplicationId { get; set; }
        public int? SalaryMatrixBandId { get; set; }
        public int? OptionNumber { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ProposedSalary { get; set; }
        public decimal? SalaryMidpoint { get; set; }
        public decimal? CompaRatio { get; set; }
        public decimal? IncreasePercentage { get; set; }
        public decimal? AnnualSalary { get; set; }
        public int? ValidationStatusId { get; set; }
        public string? Justification { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
