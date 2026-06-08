using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwSalaryMatrix
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }

        public int? CompanyId { get; set; }
        public int? DivisionId { get; set; }
        public string? MatrixName { get; set; }
        public string? MatrixCode { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int? ApprovalStatusId { get; set; }
        public bool? IsActive { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public string? CompanyName { get; set; }
        public string? DivisionName { get; set; }
        public string? CurrencyName { get; set; }
        public string? Currency { get; set; }
        public string? StatusName { get; set; }
        public string? ApproverName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
