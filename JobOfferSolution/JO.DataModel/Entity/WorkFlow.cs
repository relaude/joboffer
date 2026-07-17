using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class WorkFlow
    {
        [Key] public int Id { get; set; }
        public int? JoOfferId { get; set; }
        public int? StatusId { get; set; }
        public int? ActionId { get; set; }
    }
}
