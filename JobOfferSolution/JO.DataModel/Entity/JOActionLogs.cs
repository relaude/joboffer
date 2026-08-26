using System;
using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOActionLogs
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? ActionId { get; set; }
        public DateTime? ActionAt { get; set; }
        public int? ActionBy { get; set; }
    }
}
