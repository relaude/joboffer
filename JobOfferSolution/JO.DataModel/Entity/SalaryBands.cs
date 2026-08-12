using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SalaryBands
    {
        [Key] public int Id { get; set; }
        public int? MatrixId { get; set; }
        public int? CSGId { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Midpoint { get; set; }
        public decimal? Maximum { get; set; }
        public decimal? CompaRatio { get; set; }
    }
}
