using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CandidateStatus
    {
        [Key] public int Id { get; set; }
        public string StatusName { get; set; }
        public int OrderBy { get; set; }
    }
}
