using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOItemLetter
    {
        [Key] public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? DisplayOrder { get; set; }
        public string? ItemName { get; set; }
        public string? MessageBody { get; set; }
    }
}
