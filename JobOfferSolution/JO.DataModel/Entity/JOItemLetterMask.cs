using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JOItemLetterMask
    {
        [Key] public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? DisplayOrder { get; set; }
        public string? ItemName { get; set; }
        public string? MessageBody { get; set; }
    }
}
