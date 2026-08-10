using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CompBenItemsDto
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string ItmName { get; set; }
        public string ItmDesc { get; set; }
        public decimal Amount { get; set; }
    }
}
