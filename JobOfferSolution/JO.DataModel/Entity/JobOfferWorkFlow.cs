using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferWorkFlow
    {
        [Key] public int Id { get; set; }
        public int? CandidateMSFormRequestId { get; set; }
        public int? CandidateApplicationId { get; set; }
        public int? JobOfferAnalysisId { get; set; }
        public int? WorkFlowStatusId { get; set; }
        public int? WorkFlowActionId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
