using System;
using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOAnalysis
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public string? CandidateReamrks { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
