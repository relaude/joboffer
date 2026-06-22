using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class UserPermissionsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? PermissionCode { get; set; }
        public string? PermissionName { get; set; }
        public bool Allowed { get; set; }
    }
}
