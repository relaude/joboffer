using JO.DataModel.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.DTOs
{
    public class GroupApprovalsDto
    {
        public string? RefNum { get; set; }
        public string? ApproverType { get; set; }
        public string? ApproverName { get; set; }

        public List<VwApprovals> VwApprovals { get; set; }
    }
}
