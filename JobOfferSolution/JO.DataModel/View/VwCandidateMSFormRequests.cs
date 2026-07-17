using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwCandidateMSFormRequests
    {
        public int Id { get; set; }
        public int? CandidateId { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? RequestSentDate { get; set; }
        public DateTime? Reminder1SentDate { get; set; }
        public DateTime? Reminder2SentDate { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PositionAppliedFor { get; set; }
        public string? StatusName { get; set; }
        public string? BootstrapClass { get; set; }
        public string? TAOwner { get; set; }
        public int? StatusId { get; set; }
    }
}
