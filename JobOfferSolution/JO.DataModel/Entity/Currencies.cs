using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Currencies
    {
        [Key] public int Id { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
    }
}
