using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JORoleActionStatus
    {
        [Key] public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? RoleId { get; set; }
        public int? ActionId { get; set; }
    }
}
