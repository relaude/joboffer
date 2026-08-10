using System;

namespace JO.DataModel.View
{
    public class VwCompBenPlans
    {
        public int Id { get; set; }
        public string? PlanName { get; set; }
        public int? SGId { get; set; }
        public string? SalaryGrade { get; set; }
        public int? TypeId { get; set; }
        public string? EmpStatus { get; set; }
        public int? AreaId { get; set; }
        public string? WorkArea { get; set; }
        public int? SchedId { get; set; }
        public string? WorkSchedule { get; set; }
        public int? ShiftId { get; set; }
        public string? ShiftCode { get; set; }
        public int? ClassId { get; set; }
        public string? JobClass { get; set; }
        public int? FreqId { get; set; }
        public string? Frequency { get; set; }
        public int? MRIId { get; set; }
        public string? MRI { get; set; }
        public bool? Motorized { get; set; }
        public bool? AllowTrans { get; set; }
        public bool? AllowSpec { get; set; }
        public bool? Incentive { get; set; }
        public bool? Annual { get; set; }
        public bool? Swipe { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
