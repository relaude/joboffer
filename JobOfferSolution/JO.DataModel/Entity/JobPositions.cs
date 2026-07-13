using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobPositions
    {
        [Key] public int Id { get; set; }
        public string? PositionName { get; set; }
        public int? JobFamilyId { get; set; }
    }
}
