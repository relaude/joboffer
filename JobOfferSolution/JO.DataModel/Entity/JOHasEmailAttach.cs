using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JOHasEmailAttach
    {
        [Key] public int Id { get; set; }
        public int? JOEmailId { get; set; }
        public int? JobOfferId { get; set; }
        public string? FileName { get; set; }
        public string? RelativePath { get; set; }
    }
}
