using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompanyCompensation
    {
        [Key] public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string? CmpnyCmpnstnName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
