using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateFiles
    {
        [Key] public int Id { get; set; }
        public int? CandidateId { get; set; }
        public int? TypeId { get; set; }
        public string? FileName { get; set; }
        public string? RelativePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
