using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Requests
    {
        [Key] public int Id { get; set; }
        public int? CandidateId { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DueAt { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime? Reminder1 { get; set; }
        public DateTime? Reminder2 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
