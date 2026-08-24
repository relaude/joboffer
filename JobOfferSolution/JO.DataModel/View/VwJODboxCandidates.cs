using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJODboxCandidates
    {
        public int Id { get; set; }
        public string? RefNum { get; set; }
        public string? DboxRefNum { get; set; }
        public string? CandidateName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? WorkFlowId { get; set; }
        public string? FlowName { get; set; }
        public int? OfferRangeId { get; set; }
        public string? RangeName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
