using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Frequencies
    {
        [Key] public int Id { get; set; }
        public string? FrequencyName { get; set; }
        public int? Multiplier { get; set; }
    }
}
