using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CandidateMassUploadModalDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? ContactNumber { get; set; }

        public decimal? ExpectedSalary { get; set; }

        public int? JobFamilyId { get; set; }
        public int? JobPositionId { get; set; }
    }
}
