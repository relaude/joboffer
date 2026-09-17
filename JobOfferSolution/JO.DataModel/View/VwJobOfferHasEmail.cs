using System;

namespace JO.DataModel.View
{
    public class VwJobOfferHasEmail
    {
        public int Id { get; set; }
        public int? StatusId { get; set; }
        public string? Subject { get; set; }
        public string? StatusName { get; set; }
        public string? JORefNum { get; set; }
        public string? CandidateName { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
