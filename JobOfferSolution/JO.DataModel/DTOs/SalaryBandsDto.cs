using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class SalaryBandsDto
    {
        public int? CSGId { get; set; }
        public string? TypeName { get; set; }
        public string? GradeName { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Midpoint { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? CompaRatio { get; set; }
        public bool? LockCSGId { get; set; } = false;
    }
}
