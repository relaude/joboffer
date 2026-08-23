using System;

namespace JO.DataModel.View
{
    public class VwCompanyCompensation
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? CmpnyCmpnstnName { get; set; }
        public bool? IsActive { get; set; }
        public int ItemCount { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
