using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Discussions
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? ProposalId { get; set; }
        public int? StepId { get; set; }
        public int? ChannelId { get; set; }
        public int? ResponseId { get; set; }
        public string? Comments { get; set; }
        public string? FeedBack { get; set; }
        public DateTime? DiscussAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
