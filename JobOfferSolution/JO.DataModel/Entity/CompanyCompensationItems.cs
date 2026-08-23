using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompanyCompensationItems
    {
        [Key] public int Id { get; set; }
        public int? CmpnyCmpnstnId { get; set; }
        public int? ItemId { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public decimal? AnnualAmount { get; set; }
        public bool? IsAnalysis { get; set; }
        public bool? IsEditable { get; set; }
    }
}
