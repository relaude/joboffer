using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class DiscussionDto
    {
        public int? JobOfferId { get; set; }
        public int? ProposalId { get; set; }
        public int? StepId { get; set; }
        public int? ChannelId { get; set; }
        public int? ResponseId { get; set; }
        public string? Comments { get; set; }
        public string? FeedBack { get; set; }
        public DateTime? DiscussAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
