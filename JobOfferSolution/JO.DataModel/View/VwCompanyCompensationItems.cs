namespace JO.DataModel.View
{
    public class VwCompanyCompensationItems
    {
        public int Id { get; set; }
        public int? CmpnyCmpnstnId { get; set; }
        public string? CmpnyCmpnstnName { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? CategoryId { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public decimal? AnnualAmount { get; set; }
        public bool? IsAnalysis { get; set; }
        public bool? IsEditable { get; set; }
    }
}
