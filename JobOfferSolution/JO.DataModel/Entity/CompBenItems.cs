using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenItems
    {
        [Key] public int Id { get; set; }
        public int? PlanId { get; set; }
        public int? CatId { get; set; }
        public string? ItmName { get; set; }
        public string? ItmDesc { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Multiplier { get; set; }
        
    }
}
