using System;
using System.Collections.Generic;
using System.Text;

namespace JO.DataModel.Entity
{
    public class CompBenPlans
    {
        public int Id { get; set; }
        public string? PlanName { get; set; }
        public int? CompanyId { get; set; }
        public int? CSGId { get; set; }
        public int? TypeId { get; set; }
        public int? AreaId { get; set; }
        public int? SchedId { get; set; }
        public int? ShiftId { get; set; }
        public int? ClassId { get; set; }
        public int? FreqId { get; set; }
        public int? MRIId { get; set; }
        public bool? Motorized { get; set; }
        public bool? AllowTrans { get; set; }
        public bool? AllowSpec { get; set; }
        public bool? Incentive { get; set; }
        public bool? Annual { get; set; }
        public bool? NonSwipe { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
