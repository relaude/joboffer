using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class ReturnLogs
    {
        [Key] public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? JobOffer_Id { get; set; }
        public int? ReturnReason_Id { get; set; }
        public string? Remarks { get; set; }
    }
}
