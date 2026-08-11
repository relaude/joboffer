using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class CBPlnHasItem
    {
        [Key]
        public int Id { get; set; }
        public int? PlanId { get; set; }
        public int? ItemId { get; set; }
    }
}
