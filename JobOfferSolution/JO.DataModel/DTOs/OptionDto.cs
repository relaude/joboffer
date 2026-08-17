using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class OptionDto
    {
        public int Id { get; set; }
        public int OptionNum { get; set; }
        public decimal Increase { get; set; }
        public string IncreaseStr { get; set; }
        public decimal MbsMonthly { get; set; }
        public decimal MbsAnnualy { get; set; }
        public decimal Month13Annualy { get; set; }
        public decimal Month14Annualy { get; set; }
        public decimal AllowanceMonthly { get; set; }
        public decimal AllowanceAnnualy { get; set; }
        public decimal PSAnnualy { get; set; }
        public decimal IncentiveAnnualy { get; set; }
        public decimal PerformanceAnnualy { get; set; }
        public bool Recommend { get; set; }
    }
}
