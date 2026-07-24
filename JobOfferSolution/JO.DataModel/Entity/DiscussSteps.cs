using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class DiscussSteps
    {
        [Key] public int Id { get; set; }
        public string StepName { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
    }
}
