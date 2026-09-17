using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferHasEmail
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? CandidateId { get; set; }
        public int? StatusId { get; set; }
        public string? ToRecipient { get; set; }
        public string? CCRecipient { get; set; }
        public string? Subject { get; set; }
        public string? EmailMessage { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
