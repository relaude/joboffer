using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOffers
    {
        [Key] public int Id { get; set; }
        public string? RefNum { get; set; }
        public int? CompanyId { get; set; }
        public int? DivisionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CandidateId { get; set; }
        public int? RequestId { get; set; }
        public int? DocumentId { get; set; }
        public int? LegalId { get; set; }
        public int? StatusId { get; set; }
        public int? WorkFlowId { get; set; }
        public int? ActionId { get; set; }
        public int? NextActionId { get; set; }
        public int? Options { get; set; }
        public int? JOAnalysisId { get; set; }
        public int? CmpnyCmpnstnId { get; set; }
        public int? OfferRangeId { get; set; }
        public bool? Escalate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
