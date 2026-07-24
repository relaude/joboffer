using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Approvals
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? ProposalId { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public string? Comments { get; set; }
        public DateTime? ApproveAt { get; set; }
        public int? ApproveBy { get; set; }
    }
}
