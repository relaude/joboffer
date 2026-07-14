using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class DHJOProposal
    {
        [Key] public int Id { get; set; }
        public int?JobOfferProposalId { get; set; }
        public int? ProposalStatusId { get; set; }
        public string? Comments { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
