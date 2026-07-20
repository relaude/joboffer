using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJobOffers
    {
        public int Id { get; set; }
        public string? RefNum { get; set; }
        public string? StatusName { get; set; }
        public string? CandidateName { get; set; }
        public string? Email { get; set; }
        public string? BootstrapClass { get; set; }
        public int? StatusId { get; set; }
        public int? RequestId { get; set; }
    }
}
