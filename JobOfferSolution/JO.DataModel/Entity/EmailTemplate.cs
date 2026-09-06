using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JO.DataModel.Entity
{
    public class EmailTemplate
    {
        [Key] public int Id { get; set; }
        public int? WorkFlowId { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailMessage { get; set; }
        public string? OtherRecipient { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
