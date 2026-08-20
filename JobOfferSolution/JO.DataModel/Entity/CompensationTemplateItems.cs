using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompensationTemplateItems
    {
        [Key] public int Id { get; set; }
        public int? TemplateId { get; set; }
        public int? ItemId { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
