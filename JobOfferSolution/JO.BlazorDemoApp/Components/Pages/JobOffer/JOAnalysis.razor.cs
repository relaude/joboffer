using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.JobOffer
{
    public partial class JOAnalysis
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }

        private int currentJOCmpnyCmpnstnId;
        private int selectedCmpnyCmpnstnId;
        private int selectedJOCmpnyCmpnstnId;
        private int selectedOptionNumber = 1;
        private int ulEquivalentTotalMonthsPay = 15;

        private VwDboxCandidates candidate = new();
        private JobOffers jobOffer = new();
        private JO.DataModel.Entity.JOAnalysis joAnalysis = new();
        private VwJODboxCandidates vwjobOffer = new();
        private VwSalaryBands vwSalaryBand = new();

        private List<CompanyCompensation> companyCompensation = new();
        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOCompanyCompensationItems> joCompanyCompensationItems = new();
        private List<CompenItemCategoryDto> compenItemCategoryDto = new();

        private IEnumerable<JOCompanyCompensation> ComparativeCompensations =>
            joCompanyCompensation.OrderBy(compensation => compensation.OptionNumber);

        private IEnumerable<int> ComparativeItemIds => joCompanyCompensationItems
            .Where(item => item.ItemId.HasValue)
            .Select(item => item.ItemId!.Value)
            .Distinct()
            .OrderBy(GetCompensationItemDisplayOrder)
            .ThenBy(itemId => itemId);

        protected override async Task OnParametersSetAsync()
        {
            jobOffer = await CompensationService.GetJobOffer(jobOfferId);
            joAnalysis = await CompensationService.GetJOAnalysis(jobOfferId);
            vwjobOffer = await CompensationService.GetVwJODboxCandidates(jobOfferId);
            candidate = await CandidateService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwSalaryBand = await CompensationService.GetVwSalaryBand(jobOffer.CompanyId.GetValueOrDefault(), candidate.CSGId.GetValueOrDefault());

            companyCompensation = await CompensationService.GetCompanyCompensation(jobOffer.CompanyId.GetValueOrDefault());
            joCompanyCompensation = await CompensationService.GetJOCompanyCompensation(jobOfferId);
            joCompanyCompensationItems = await CompensationService.GetJOCmpnyCompensationItems(jobOfferId);
            compenItemCategoryDto = await CompensationService.SetUpCompenItemCategoryDto();

            SetDefaultSelection();
            await LoadSelectedCompensationItems();
        }

        private void SetDefaultSelection()
        {
            var selectedCompensation = joCompanyCompensation
                .Where(compensation => compensation.OptionNumber > 0)
                .OrderBy(compensation => compensation.OptionNumber)
                .FirstOrDefault();

            currentJOCmpnyCmpnstnId = joCompanyCompensation
                .FirstOrDefault(compensation => compensation.OptionNumber == 0)?.Id ?? 0;
            selectedOptionNumber = selectedCompensation?.OptionNumber.GetValueOrDefault() ?? 0;
            selectedJOCmpnyCmpnstnId = selectedCompensation?.Id ?? 0;
            selectedCmpnyCmpnstnId = selectedCompensation?.CmpnyCmpnstnId ?? 0;
        }

        private async Task ClickOptionTab(JOCompanyCompensation compensation)
        {
            selectedOptionNumber = compensation.OptionNumber.GetValueOrDefault();
            selectedJOCmpnyCmpnstnId = compensation.Id;
            selectedCmpnyCmpnstnId = compensation.CmpnyCmpnstnId.GetValueOrDefault();
            await LoadSelectedCompensationItems();
        }

        private async Task LoadSelectedCompensationItems()
        {
            vwCompanyCompensationItems = selectedCmpnyCmpnstnId > 0
                ? await CompensationService.GetVwCompanyCompensationItems(selectedCmpnyCmpnstnId)
                : new List<VwCompanyCompensationItems>();
        }

        private string GetCompensationItemName(int itemId)
        {
            return compenItemCategoryDto
                .SelectMany(category => category.CompensationItemDtos)
                .FirstOrDefault(item => item.Id == itemId)?.ItemName
                ?? $"Item {itemId}";
        }

        private int GetCompensationItemDisplayOrder(int itemId)
        {
            return compenItemCategoryDto
                .SelectMany(category => category.CompensationItemDtos)
                .FirstOrDefault(item => item.Id == itemId)?.DisplayOrder
                ?? int.MaxValue;
        }

        private string GetCompanyCompensationName(int? companyCompensationId)
        {
            return companyCompensation
                .FirstOrDefault(compensation => compensation.Id == companyCompensationId)?.CmpnyCmpnstnName
                ?? "Compensation";
        }

        private static string? GetOptionHighlightClass(JOCompanyCompensation compensation)
        {
            var optionNumber = compensation.OptionNumber.GetValueOrDefault();

            if (optionNumber <= 0)
                return null;

            return optionNumber % 2 == 0
                ? "is-option-highlight-alt"
                : "is-option-highlight";
        }

        private static string FormatPercent(decimal? value)
        {
            return value.HasValue ? $"{value.Value:N2}%" : "-";
        }

        private decimal? ComputeUlEquivalentMonthlyBasic()
        {
            int currentMonth = MonthPayToInt();

            decimal ulEquivalentMonthlybasic = (candidate.CurrentMonthlyBasicSalary.GetValueOrDefault() * currentMonth) / ulEquivalentTotalMonthsPay;
            return ulEquivalentMonthlybasic;
        }

        private int MonthPayToInt()
        {
            return candidate.GuaranteedMonthsPay switch
            {
                "13th Month Pay" => 13,
                "14th Month Pay" => 14,
                "15th Month Pay" => 15,
                "16th Month Pay" => 16,
                _ => 0
            };
        }
    }
}
