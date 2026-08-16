using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOffers
    {
        [Key] public int Id { get; set; }
        public string? RefNum { get; set; }
        public int? CandidateId { get; set; }
        public int? RequestId { get; set; }
        public int? DocumentId { get; set; }
        public int? LegalId { get; set; }
        public int? StatusId { get; set; }
        public int? Options { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
