using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateTempData
    {
        [Key] public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? PositionAppliedFor { get; set; }
        public int? JobPositionId { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public bool? HasErrors { get; set; }
        public string? Errors { get; set; }
        public int? MassUploadLogId { get; set; }
    }
}
