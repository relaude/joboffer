using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Departments
    {
        [Key] public int Id { get; set; }
        public string? DepartmentName { get; set; }
        public int? Division_Id { get; set; }
    }
}
