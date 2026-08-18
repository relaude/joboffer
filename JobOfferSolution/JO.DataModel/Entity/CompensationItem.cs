using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompensationItem
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
