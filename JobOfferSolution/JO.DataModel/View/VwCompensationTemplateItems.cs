namespace JO.DataModel.View
{
    public class VwCompensationTemplateItems
    {
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public int? ItemId { get; set; }
        public bool? IsEnabled { get; set; }
        public string? TemplateName { get; set; }
        public string? ItemName { get; set; }
        public decimal? Monthly { get; set; }
        public decimal? Annualy { get; set; }
        public bool? IsAnalysis { get; set; }
    }
}
