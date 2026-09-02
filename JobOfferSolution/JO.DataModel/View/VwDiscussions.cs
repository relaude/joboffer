using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwDiscussions
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? ProposalId { get; set; }
        public int? StatusId { get; set; }
        public int? DeclineReasonId { get; set; }
        public string? DeclineRemarks { get; set; }

        public int? StepId { get; set; }
        public int? ChannelId { get; set; }
        public int? ResponseId { get; set; }
        public string? Comments { get; set; }
        public string? FeedBack { get; set; }
        public DateTime? DiscussAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? RefNum { get; set; }
        public int? OptionNumber { get; set; }
        public decimal? ProposedSalary { get; set; }
        public string? StatusName { get; set; }
        public string? ReasonName { get; set; }
    }
}
