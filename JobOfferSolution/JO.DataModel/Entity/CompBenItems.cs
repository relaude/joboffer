using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenItems
    {
        [Key] public int Id { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public int? TypeId { get; set; }
    }
}
