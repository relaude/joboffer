using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenMRI
    {
        [Key] public int Id { get; set; }
        public string? MRIName { get; set; }
    }
}
