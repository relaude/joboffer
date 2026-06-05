using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Divisions
    {
        [Key] public int Id { get; set; }
        public string? DivisionCode { get; set; }
        public string? DivisionName { get; set; }
        public int? CompanyId { get; set; }
    }
}
