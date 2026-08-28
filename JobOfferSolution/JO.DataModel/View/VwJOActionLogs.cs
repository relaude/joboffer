namespace JO.DataModel.View
{
    public class VwJOActionLogs
    {
        public int Id { get; set; }
        public int? JobOfferId { get; set; }
        public int? RoleId { get; set; }
        public int? ActionId { get; set; }
        public DateTime? ActionAt { get; set; }
        public int? ActionBy { get; set; }
        public string? ActionName { get; set; }
        public string? ActionByName { get; set; }
        public string? RoleName { get; set; }
    }
}
