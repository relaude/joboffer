using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwApprovals
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? StatusId { get; set; }
        public string? RefNum { get; set; }
        public string? StatusName { get; set; }
        public string? ApproverType { get; set; }
        public int? OptionNum { get; set; }
        public decimal? ProposeSalary { get; set; }
        public decimal? Annual { get; set; }
        public decimal? Increase { get; set; }
        public decimal? CompaRatio { get; set; }
        public bool? Recommend { get; set; }
        public string? SalaryBand { get; set; }
        public string? ApproverName { get; set; }
        public string? Comments { get; set; }
        public DateTime? ApproveAt { get; set; }
        public string? BootstrapClass { get; set; }
    }
}
