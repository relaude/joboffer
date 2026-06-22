using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class Permissions
    {
        [Key] public int Id { get; set; }
        public string? PermissionCode { get; set; }
        public string? PermissionName { get; set; }
    }
}
