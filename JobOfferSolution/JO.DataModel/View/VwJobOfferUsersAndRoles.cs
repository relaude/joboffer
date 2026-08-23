using System;

namespace JO.DataModel.View
{
    public class VwJobOfferUsersAndRoles
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? RoleName { get; set; }
    }
}
