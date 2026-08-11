using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CBPckHasItem
    {
        [Key] public int Id { get; set; }
        public int? PckgId { get; set; }
        public int? ItemId { get; set; }
    }
}
