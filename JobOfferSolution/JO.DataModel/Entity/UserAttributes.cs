using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class UserAttributes
    {
        [Key] public int Id { get; set; }
        public int? JobOfferUserId { get; set; }
        public string? AttributeName { get; set; }
        public string? AttributeValue { get; set; }
    }
}
