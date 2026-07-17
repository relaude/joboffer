using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class WorkFlowStatus
    {
        [Key] public int Id { get; set; }
        public string? WorkFlowName { get; set; }
        public string? ShortName { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
