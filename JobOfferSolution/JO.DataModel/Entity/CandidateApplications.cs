using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateApplications
    {
        [Key] public int Id { get; set; }
        public int? CandidateId { get; set; }
        public int? MSFormRequestId { get; set; }
        public int? HiringDivisionId { get; set; }
        public int? CurrencyId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
