using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenSched
    {
        [Key] public int Id { get; set; }
        public string SchedName { get; set; }
    }
}
