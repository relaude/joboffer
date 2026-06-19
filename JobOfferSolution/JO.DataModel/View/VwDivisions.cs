using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwDivisions
    {
        public int Id { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
    }
}
