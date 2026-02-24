using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOffers : BaseEntity
    {
        public string? JobOfferNumber { get; set; }
        public int? Candidate_Id { get; set; }
        public int? JobPosition_Id { get; set; }
        public int? Department_Id { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? SigningBonus { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? ProposedStartDate { get; set; }
        public string? Remarks { get; set; }
        public int? MainStatus_Id { get; set; }
    }
}
