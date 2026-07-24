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
        public int? StepId { get; set; }
        public int? ChannelId { get; set; }
        public int? ResponseId { get; set; }
        public string? Comments { get; set; }
        public string? FeedBack { get; set; }
        public DateTime? DiscussAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? RefNum { get; set; }
        public int? OptionNum { get; set; }
        public decimal? ProposeSalary { get; set; }
        public string? StepName { get; set; }
        public string? Icon { get; set; }
        public string? ChannelName { get; set; }
        public string? ResponseName { get; set; }
    }
}
