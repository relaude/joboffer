using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class DboxCandidates
    {
        [Key] public int Id { get; set; }
        public int? ResponseId { get; set; }
        public int? DboxId { get; set; }
        public string? DboxRefNum { get; set; }
        public int? CSGId { get; set; }
        public string? CandidateName { get; set; }
        public string? Company { get; set; }
        public string? Division { get; set; }
        public string? Department { get; set; }
        public string? CostCenter { get; set; }
        public string? JobLevel { get; set; }
        public string? JobPosition { get; set; }
        public string? EmailAddress { get; set; }
        public string? ContactNumber { get; set; }
    }
}
