using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CompensationItemDto
    {
        public int Id { get; set; }
        public string? ItemName { get; set; }
        public int? CategoryId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
