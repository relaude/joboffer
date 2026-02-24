using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Departments : BaseEntity
    {
        public string? DepartmentName { get; set; }
        public int? Division_Id { get; set; }
    }
}
