using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferWorkFlow
    {
        public int Id { get; set; }
        public int? CandidateMSFormRequestId { get; set; }
        public int? CandidateApplicationId { get; set; }
        public int? JobOfferAnalysisId { get; set; }
        public int? WorkFlowStatusId { get; set; }
        public int? WorkFlowActionId { get; set; }
        public string? ShortName { get; set; }
        public string? Icon { get; set; }
        public int? DisplayOrder { get; set; }
        public string? ActionName { get; set; }
        public string? ActionClass { get; set; }
    }
}
