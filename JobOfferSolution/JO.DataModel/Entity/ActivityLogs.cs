using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JO.DataModel.Entity
{
    public class ActivityLogs
    {
        [Key] public int Id { get; set; }
        public int? JobOffer_Id { get; set; }
        public int? Activity_Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
