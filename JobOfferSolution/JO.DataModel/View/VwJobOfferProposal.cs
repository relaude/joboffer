using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferProposal
    {
        public int Id { get; set; }
        public int? JobOfferAnalysisId { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? OptionNumber { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ProposedSalary { get; set; }
        public decimal? SalaryMidpoint { get; set; }
        public decimal? CompaRatio { get; set; }
        public decimal? IncreasePercentage { get; set; }
        public decimal? AnnualSalary { get; set; }
        public string? ValidationStatusName { get; set; }
        public string? Justification { get; set; }
        public string? DHProposalStatus { get; set; }
        public string? DHComments { get; set; }
        public int? DHProposalStatusId { get; set; }

        [NotMapped] public int? ProposalStatusId { get; set; }
        [NotMapped] public string? Comments { get; set; }
    }
}
