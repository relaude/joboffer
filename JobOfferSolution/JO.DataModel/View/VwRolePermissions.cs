using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.View
{
    public class VwRolePermissions
    {
        public int Id { get; set; }
        public string? AspNetRoleId { get; set; }
        public int? PermissionId { get; set; }
        public int? JORoleId { get; set; }
        public string? RoleName { get; set; }
        public string? PermissionCode { get; set; }
        public string? PermissionName { get; set; }
    }
}
