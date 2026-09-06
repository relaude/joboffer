using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class EmailTemplateRoles
    {
        [Key] public int Id { get; set; }
        public int? EmailTemplateId { get; set; }
        public int? RoleId { get; set; }
    }
}
