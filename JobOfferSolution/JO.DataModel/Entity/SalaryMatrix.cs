using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SalaryMatrix
    {
        [Key] public int Id { get; set; }
        public int? CompanyId { get; set; }
        //public int? DivisionId { get; set; }
        //public string? MatrixName { get; set; }
        //public string? MatrixCode { get; set; }
        //public int? CurrencyId { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        //public int? ApprovalStatusId { get; set; }
        public bool? IsActive { get; set; }
        //public int? ApprovedByUserId { get; set; }
        //public DateTime? ApprovedAt { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
