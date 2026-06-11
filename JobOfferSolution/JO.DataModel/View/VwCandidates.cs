using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwCandidates
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string PositionAppliedFor { get; set; }
        public string LastPositionHeld { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string CurrentEmployer { get; set; }
        public int? EmploymentStatus_Id { get; set; }
        public int? WorkExperience_Id { get; set; }
        public bool? IsActive { get; set; }
        public int? JobPosition_Id { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }

        public string? EmploymentStatusName { get; set; }
        public string? WorkExperienceName { get; set; }
        public string? JobPositionName { get; set; }
    }
}
