using System;
using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompensationPackage
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? PckgTempId { get; set; }
        public string? OptionType { get; set; }
        public int? OptionNumber { get; set; }
        public decimal? IncreasePercent { get; set; }
        public decimal? MonthlyBasic { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
