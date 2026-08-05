using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwSalaryBands
    {
        public int Id { get; set; }
        public int? MatrixId { get; set; }
        public int? CSGId { get; set; }
        public decimal? Minimum { get; set; }
        public decimal? Midpoint { get; set; }
        public decimal? Maximum { get; set; }
        public string? TypeName { get; set; }
        public string? GradeName { get; set; }
    }
}
