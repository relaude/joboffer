using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwActivityLogs
    {
        public int Id { get; set; }
        public int? JobOffer_Id { get; set; }
        public int? Activity_Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public string? ActionName { get; set; }
        public string? ActionBy { get; set; }
    }
}
