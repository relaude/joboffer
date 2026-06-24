using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJOUsersInRoles
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? RoleCategory { get; set; }
        public string? AspNetRoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
