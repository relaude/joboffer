using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class ValidationStatus
    {
        [Key] public int Id { get; set; }
        public string? ValidationStatusName { get; set; }
        public string? RiskLevelName { get; set; }
        public string? RiskLevelReason { get; set; }
        public int? OrderBy { get; set; }
    }
}
