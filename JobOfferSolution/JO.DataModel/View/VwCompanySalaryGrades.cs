using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwCompanySalaryGrades
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? GradeId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? TypeName  { get; set; }
        public string? GradeName { get; set; }
    }
}
