using JO.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class JobOfferUsers : BaseEntity
    {
        public string? AspNetUser_Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
    }
}
