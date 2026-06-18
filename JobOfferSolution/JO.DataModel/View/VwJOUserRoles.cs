using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwJOUserRoles
    {
        public int Id { get; set; }
        public string? AspNetRoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleCategory { get; set; }
        public int? OrderBy { get; set; }
    }
}
