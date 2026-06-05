using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobPositionGrades
    {
        [Key] public int Id { get; set; }
        public string? PositionGrade { get; set; }
    }
}
