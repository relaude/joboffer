using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Proposal
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? SalaryBandId { get; set; }
        public int? OptionNum { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ProposeSalary { get; set; }
        public decimal? CompaRatio { get; set; }
        public decimal? Increase { get; set; }
        public decimal? Annual { get; set; }
        public int? PackageId { get; set; }
        public bool? Recommend { get; set; }
        public int? StatusId { get; set; }
        public bool? Escalate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
