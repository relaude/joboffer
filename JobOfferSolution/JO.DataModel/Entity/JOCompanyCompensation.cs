using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOCompanyCompensation
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? JOAnalysisId { get; set; }
        public int? CSGId { get; set; }
        public int? CmpnyCmpnstnId { get; set; }
        public int? OptionNumber { get; set; }
        public decimal? CurrentSalary { get; set; }
        public decimal? ProposedSalary { get; set; }
        public decimal? Increase { get; set; }
        public decimal? TotalMonthly { get; set; }
        public decimal? TotalAnnually { get; set; }
        public decimal? DiffTotalMonthly { get; set; }
        public decimal? DiffTotalAnnually { get; set; }
        public string? BandStatus { get; set; }
        public bool? Escalate { get; set; }
        public int? OfferRangeId { get; set; }
        public int? Incumbents { get; set; }
        public string? Remarks { get; set; }
        public bool? Accepted { get; set; }
        public bool? Declined { get; set; }
        public bool? ForNegotiation { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
