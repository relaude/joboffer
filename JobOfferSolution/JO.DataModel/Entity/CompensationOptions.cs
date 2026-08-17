using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompensationOptions
    {
        [Key] public int Id { get; set; }
        public int? PackageId { get; set; }
        public int? ItemId { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public decimal? AnnualAmount { get; set; }
    }
}
