using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CandidateMSFormRequestDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateEmail { get; set; }
        public DateTime DueDate { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public bool Reminder1 { get; set; }
        public bool Reminder2 { get; set; }
        public int CreatedBy { get; set; }
    }
}
