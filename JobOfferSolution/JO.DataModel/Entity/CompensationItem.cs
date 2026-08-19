using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompensationItem
    {
        [Key] public int Id { get; set; }
        public string? ItemName { get; set; }
        public int? CategoryId { get; set; }
        public decimal? Monthly { get; set; }
        public decimal? Annualy { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
