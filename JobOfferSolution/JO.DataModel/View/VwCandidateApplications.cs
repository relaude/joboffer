using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwCandidateApplications
    {
        public int Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public string? CandidateEmail { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public string? PositionAppliedFor { get; set; }
        public int? SalaryMatrixId { get; set; }
        public string? MatrixCode { get; set; }
        public string? MatrixName { get; set; }
        public string? Currency { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
