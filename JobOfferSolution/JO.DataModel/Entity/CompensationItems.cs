using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CompensationItems
    {
        [Key] public int Id { get; set; }
        public string? ItemName { get; set; }
        public int? CategoryId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
