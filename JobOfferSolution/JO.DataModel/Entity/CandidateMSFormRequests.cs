using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateMSFormRequests
    {
        [Key] public int Id { get; set; }
        public int? CandidateId { get; set; }
        public int? CandidateApplicationId { get; set; }
        public DateTime? RequestSentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? SubmissionReferenceNo { get; set; }
        public int? StatusId { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public DateTime? Reminder1SentDate { get; set; }
        public DateTime? Reminder2SentDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
