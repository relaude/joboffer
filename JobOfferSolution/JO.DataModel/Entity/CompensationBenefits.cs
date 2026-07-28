using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompensationBenefits
    {
        [Key] public int Id { get; set; }
        public int? PackageId { get; set; }
        public int? CompBenItemId { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public bool? IsTaxable { get; set; }
        public decimal? Tax { get; set; }
        public bool? IsRecurring { get; set; }
        public int? FrequencyId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
