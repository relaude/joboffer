using JO.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOfferTransactions : JobOfferTransactions
    {
        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string StatusName { get; set; }
    }
}
