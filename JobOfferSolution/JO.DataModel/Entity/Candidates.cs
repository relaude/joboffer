using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Candidates 
    {
        [Key] public int Id { get; set; }
        public string? FName { get; set; }
        public string? MName { get; set; }
        public string? LName { get; set; }
        public string? Email { get; set; }
        public string? Contact { get; set; }
        public string? ApplyFor { get; set; }
        public string? LstPosition { get; set; }
        public string? Employer { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Expected { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
