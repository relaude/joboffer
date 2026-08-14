using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwPckgTemp
    {
        public int Id { get; set; }
        public string? TempName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
