using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class ChannelTypes
    {
        [Key] public int Id { get; set; }
        public string? ChannelName { get; set; }
    }
}
