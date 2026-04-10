using JO.DataModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class GroupedActivityLogs
    {
        public DateTime Date { get; set; }
        public List<VwActivityLogs> Logs { get; set; }
    }
}
