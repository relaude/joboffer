using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class ComparisonDto
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public string Compensation { get; set; }
        public decimal? CurMonthy { get; set; }
        public decimal? CurAnnual { get; set; }
        public string? CurRemarks { get; set; }
        public decimal? OptMonthy { get; set; }
        public decimal? OptAnnual { get; set; }
        public string? OptRemarks { get; set; }

        public List<OptionDto> OptionsDto { get; set; } = new();
    }
}
