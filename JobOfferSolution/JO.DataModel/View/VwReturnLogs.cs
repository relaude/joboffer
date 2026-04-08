using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwReturnLogs
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? JobOffer_Id { get; set; }
        public int? ReturnReason_Id { get; set; }
        public string? Remarks { get; set; }
        public string? ReasonName { get; set; }
        public string? ReturnBy { get; set; }
    }
}
