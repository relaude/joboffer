using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Candidates : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsHROD { get; set; }
    }
}
