using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwPckgTempHasItms
    {
        public int Id { get; set; }
        public int? TempId { get; set; }
        public int? ItemId { get; set; }
        public string? TempName { get; set; }
        public string? ItemName { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? Analysis { get; set; }
        public decimal? Monthly { get; set; }
        public decimal? Annualy { get; set; }
    }
}
