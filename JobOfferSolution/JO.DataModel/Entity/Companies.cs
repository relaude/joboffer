using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Companies
    {
        [Key] public int Id { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
    }
}
