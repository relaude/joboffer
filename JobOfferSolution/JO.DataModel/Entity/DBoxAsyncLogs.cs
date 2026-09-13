using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class DBoxAsyncLogs
    {
        [Key] public int Id { get; set; }
        public DateTime? SyncDate { get; set; }
        public int? TotalRows { get; set; }
    }
}
