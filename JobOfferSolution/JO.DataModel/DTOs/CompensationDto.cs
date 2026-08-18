using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class CompensationDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal CurrentMonthly { get; set; }
        public decimal CurrentAnnual { get; set; }
        public decimal OptionMonthly { get; set; }
        public decimal OptionAnnual { get; set; }
    }
}
