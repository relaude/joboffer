using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferDocuments
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? CandidateId { get; set; }
        public int? SalaryOptionId { get; set; }
        public int? DocumentType { get; set; }
        public string? FileName { get; set; }
        public string? RelativeFilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
