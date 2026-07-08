using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwCompensationBenefits
    {
        public int Id { get; set; }
        public int? PackageId { get; set; }
        public string? PackageName { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? TypeName { get; set; }
        public string? Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public bool? IsTaxable { get; set; }
        public bool? IsRecurring { get; set; }
        public string? FrequencyName { get; set; }
        public int? Multiplier { get; set; }
        public bool? ActiveItem { get; set; }
        public bool? ActivePackage { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
