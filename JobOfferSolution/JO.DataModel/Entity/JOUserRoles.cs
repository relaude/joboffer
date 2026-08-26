using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOUserRoles
    {
        [Key] public int Id { get; set; }
        public string? AspNetRoleId { get; set; }
        public int? RoleCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public int? OrderBy { get; set; }
        public string? Description { get; set; }
    }
}
