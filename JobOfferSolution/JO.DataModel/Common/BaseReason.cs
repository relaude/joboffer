using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Common
{
    public class BaseReason
    {
        [Key] public int Id { get; set; }
        public string Reason { get; set; }
    }
}
