using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class UserDivisionAccess
    {
        [Key] public int Id { get; set; }
        public int? JobOfferUserId { get; set; }
        public int? DivisionId { get; set; }
    }
}
