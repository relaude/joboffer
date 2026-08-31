namespace JO.DataModel.View
{
    public class VwJOApprovalFlow
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsAproved { get; set; }
        public string? RoleName { get; set; }
    }
}
