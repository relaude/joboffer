using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOffers
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
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
        public string? CandidateName { get; set; }
        public string? PositionName { get; set; }
        public string? DepartmentName { get; set; }
        public string? StatusName { get; set; }
    }
}
