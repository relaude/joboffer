using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJODboxCandidates
    {
        public int Id { get; set; }
        public string? RefNum { get; set; }
        public string? CandidateName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? BootstrapClass { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
