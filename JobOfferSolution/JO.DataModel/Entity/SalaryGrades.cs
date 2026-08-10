using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class SalaryGrades
    {
        [Key] public int Id { get; set; }
        public int? TypeId { get; set; }
        public string? GradeName { get; set; }
    }
}
