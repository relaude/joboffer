using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenPackages
    {
        [Key] public int Id { get; set; }
        public string? PackageName { get; set; }
        public bool? IsActive { get; set; }
    }
}
