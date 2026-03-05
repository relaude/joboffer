using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class MainStatus : BaseEntity
    {
        public string StatusName { get; set; }
        public int OrderBy { get; set; }
    }
}
