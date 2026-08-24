using System.ComponentModel.DataAnnotations;

namespace JO.DataModel.Entity
{
    public class JOWorkFlowStatus
    {
        [Key] public int Id { get; set; }
        public string? FlowName { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
