using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwSalaryMatrix
    {
        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedByName { get; set; }
        public int? BandCount { get; set; }
    }
}
