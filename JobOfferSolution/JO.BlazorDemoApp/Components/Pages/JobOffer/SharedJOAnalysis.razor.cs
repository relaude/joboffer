using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class SharedJOAnalysis
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        [Parameter, EditorRequired] public VwDboxCandidates Candidate { get; set; } = new();
        [Parameter, EditorRequired] public VwSalaryBands SalaryBand { get; set; } = new();
        [Parameter, EditorRequired] public JO.DataModel.Entity.JOAnalysis Analysis { get; set; } = new();
        [Parameter, EditorRequired] public IReadOnlyList<CompanyCompensation> CompanyCompensations { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<JOCompanyCompensation> JOCompanyCompensations { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<JOCompanyCompensationItems> JOCompanyCompensationItems { get; set; } = [];
        [Parameter, EditorRequired] public IReadOnlyList<CompenItemCategoryDto> CompensationItemCategories { get; set; } = [];
        [Parameter] public string ComponentId { get; set; } = "sharedJOAnalysis";

        private int currentId;
        private int selectedId;
        private int selectedOptionNumber;
        private int ulMonthsPay = 15;
        private string ComparativeViewId => $"{ComponentId}-comparative-view";
        private IEnumerable<JOCompanyCompensation> ComparativeCompensations =>
            JOCompanyCompensations.OrderBy(x => x.OptionNumber);
        private IEnumerable<int> ComparativeItemIds => JOCompanyCompensationItems
            .Where(x => x.ItemId.HasValue).Select(x => x.ItemId!.Value).Distinct()
            .OrderBy(GetItemOrder).ThenBy(x => x);

        protected override void OnParametersSet()
        {
            currentId = JOCompanyCompensations.FirstOrDefault(x => x.OptionNumber == 0)?.Id ?? 0;
            var selected = JOCompanyCompensations.FirstOrDefault(x => x.OptionNumber == selectedOptionNumber)
                ?? JOCompanyCompensations.Where(x => x.OptionNumber > 0).OrderBy(x => x.OptionNumber).FirstOrDefault();
            selectedId = selected?.Id ?? 0;
            selectedOptionNumber = selected?.OptionNumber ?? 0;
        }

        private void SelectOption(JOCompanyCompensation value)
        {
            selectedId = value.Id;
            selectedOptionNumber = value.OptionNumber ?? 0;
        }

        private string GetCompanyName(int? id) => CompanyCompensations
            .FirstOrDefault(x => x.Id == id)?.CmpnyCmpnstnName ?? "Compensation";
        private string GetItemName(int id) => CompensationItemCategories.SelectMany(x => x.CompensationItemDtos)
            .FirstOrDefault(x => x.Id == id)?.ItemName ?? $"Item {id}";
        private int GetItemOrder(int id) => CompensationItemCategories.SelectMany(x => x.CompensationItemDtos)
            .FirstOrDefault(x => x.Id == id)?.DisplayOrder ?? int.MaxValue;
        private static string Percent(decimal? value) => value.HasValue ? $"{value:N2}%" : "-";
        private static string? Highlight(JOCompanyCompensation value) => value.OptionNumber switch
        {
            null or <= 0 => null,
            var number when number % 2 == 0 => "is-option-highlight-alt",
            _ => "is-option-highlight"
        };
        private int CurrentMonthsPay() => Candidate.GuaranteedMonthsPay switch
        {
            "13th Month Pay" => 13, "14th Month Pay" => 14,
            "15th Month Pay" => 15, "16th Month Pay" => 16, _ => 0
        };
        private decimal UlMonthlyBasic() => ulMonthsPay == 0 ? 0
            : Candidate.CurrentMonthlyBasicSalary.GetValueOrDefault() * CurrentMonthsPay() / ulMonthsPay;
    }
}
