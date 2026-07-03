using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Candidates : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? PositionAppliedFor { get; set; }
        public string? LastPositionHeld { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string? CurrentEmployer { get; set; }
        public decimal? CurrentSalary { get; set; }
        public int EmploymentStatus_Id { get; set; }
        public int WorkExperience_Id { get; set; }
        public bool? IsActive { get; set; }
        public int? JobPosition_Id { get; set; }
    }
}
