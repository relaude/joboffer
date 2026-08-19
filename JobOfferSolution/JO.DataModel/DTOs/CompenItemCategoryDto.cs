using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CompenItemCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<CompensationItemDto> CompensationItemDtos { get; set; }
    }
}
