using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SalaryBandStatus
    {
        [Key] public int Id { get; set; }
        public string? StatusName { get; set; }
        public string? RiskLevelName { get; set; }
        public string? RiskLevelReason { get; set; }
        public int? OrderBy { get; set; }
    }
}
