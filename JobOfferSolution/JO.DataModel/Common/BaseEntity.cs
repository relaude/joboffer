using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Common
{
    public class BaseEntity
    {
        [Key] public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
