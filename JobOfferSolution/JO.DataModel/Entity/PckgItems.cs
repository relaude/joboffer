using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class PckgItems
    {
        [Key] public int Id { get; set; }
        public int? CompenItemId { get; set; }
        public string? ItemName { get; set; }
        public bool? Analysis { get; set; }
        public decimal? Monthly { get; set; }
        public decimal? Annualy { get; set; }
    }
}
