using System;

namespace JO.DataModel.View
{
    public class VwEmailTemplate
    {
        public int Id { get; set; }
        public string? EmailSubject { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
