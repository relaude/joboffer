using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class UserPermissions
    {
        [Key] public int Id { get; set; }
        public int? JobOfferUserId { get; set; }
        public int? PermissionId { get; set; }
    }
}
