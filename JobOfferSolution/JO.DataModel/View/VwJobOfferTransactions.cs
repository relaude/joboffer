using JO.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferTransactions 
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string TransactionNumber { get; set; }
        public int MainStatus_Id { get; set; }
        public int Candidate_Id { get; set; }
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string StatusName { get; set; }
        public bool? IsHROD { get; set; }
    }
}
