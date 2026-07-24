using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandResponse
    {
        [Key] public int Id { get; set; }
        public string? ResponseName { get; set; }
    }
}
