using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateMassUploadLogs
    {
        [Key] public int Id { get; set; }
        public int? TotalExcelItems { get; set; }
        public int? TotalCandidateTransfer { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
