using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SubmittedDocuments
    {
        [Key] public int Id { get; set; }
        public int? CandidateId { get; set; }
        public int? MSFormRequestId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int? FileSize { get; set; }
        public int? FileTypeId { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
}
