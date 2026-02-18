using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferTransactions : BaseEntity
    {
        public string TransactionNumber { get; set; }
        public int MainStatus_Id { get; set; }
        public int Candidate_Id { get; set; }
    }
}
