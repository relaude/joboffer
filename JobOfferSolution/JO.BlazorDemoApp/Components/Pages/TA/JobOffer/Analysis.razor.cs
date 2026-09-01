using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Contracts;

namespace JO.BlazorDemoApp.Components.Pages.TA.JobOffer
{
    public partial class Analysis
    {
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        [Inject] private ICandidateService CandidateService { get; set; } = default!;
        [Inject] private ICompensationService CompensationService { get; set; } = default!;

        [Parameter] public int jobOfferId { get; set; }

        private int userId = 0;
        private int currentJOCmpnyCmpnstnId = 0;
        private int selectedCmpnyCmpnstnId = 0;
        private int selectedJOCmpnyCmpnstnId = 0;
        private int selectedOptionNumber = 1;
        private int ulEquivalentTotalMonthsPay = 15;
        private string taPartnerRemarks = string.Empty;

        private VwDboxCandidates candidate = new();
        private JobOffers jobOffer = new();
        private JOAnalysis joAnalysis = new();
        private VwJODboxCandidates vwjobOffer = new();
        private VwSalaryBands vwSalaryBand = new();

        private List<CompanyCompensation> companyCompensation = new();
        private List<VwCompanyCompensationItems> vwCompanyCompensationItems = new();
        private List<JOCompanyCompensation> joCompanyCompensation = new();
        private List<JOCompanyCompensationItems> joCompanyCompensationItems = new();
        private List<CompenItemCategoryDto> compenItemCategoryDto = new();

        private IEnumerable<JOCompanyCompensation> ComparativeCompensations =>
            joCompanyCompensation.OrderBy(jo => jo.OptionNumber);

        private IEnumerable<int> ComparativeItemIds => joCompanyCompensationItems
            .Where(jo => jo.ItemId.HasValue)
            .Select(jo => jo.ItemId!.Value)
            .Distinct()
            .OrderBy(GetCompensationItemDisplayOrder)
            .ThenBy(itemId => itemId);

        protected override async Task OnParametersSetAsync()
        {
            userId = await AccountService.GetJobOfferUserId();
            jobOffer = await CompensationService.GetJobOffer(jobOfferId);
            joAnalysis = await CompensationService.GetJOAnalysis(jobOfferId);
            vwjobOffer = await CompensationService.GetVwJODboxCandidate(jobOfferId);
            candidate = await CandidateService.GetVwDboxCandidate(jobOffer.CandidateId.GetValueOrDefault());
            vwSalaryBand = await CompensationService.GetVwSalaryBand(jobOffer.CompanyId.GetValueOrDefault(), candidate.CSGId.GetValueOrDefault());
            companyCompensation = await CompensationService.GetCompanyCompensation(jobOffer.CompanyId.GetValueOrDefault());
            joCompanyCompensation = await CompensationService.GetJOCompanyCompensation(jobOfferId);
            joCompanyCompensationItems = await CompensationService.GetJOCmpnyCompensationItems(jobOfferId);
            compenItemCategoryDto = await CompensationService.SetUpCompenItemCategoryDto();

            SetIDdefaultValue();

            vwCompanyCompensationItems = await CompensationService
                .GetVwCompanyCompensationItems(selectedCmpnyCmpnstnId);
            
            ReFillAllJOCmpnyCompensationItems();
        }

        private void SetIDdefaultValue()
        {
            var selectedCompensation = joCompanyCompensation
                .FirstOrDefault(jo => jo.OptionNumber == selectedOptionNumber);

            currentJOCmpnyCmpnstnId = joCompanyCompensation
                .FirstOrDefault(jo => jo.OptionNumber == 0)?.Id ?? 0;

            selectedJOCmpnyCmpnstnId = selectedCompensation?.Id ?? 0;

            selectedCmpnyCmpnstnId = selectedCompensation?.CmpnyCmpnstnId
                ?? companyCompensation.FirstOrDefault()?.Id
                ?? 0;
        }

        private void ClickOptionTab(JOCompanyCompensation compensation)
        {
            selectedOptionNumber = compensation.OptionNumber.GetValueOrDefault();
            selectedJOCmpnyCmpnstnId = compensation.Id;
        }

        private async Task OnSelectCompensation()
        {
            vwCompanyCompensationItems = await CompensationService.GetVwCompanyCompensationItems(selectedCmpnyCmpnstnId);

            ReFillAllJOCmpnyCompensationItems();
        }

        private void ReFillAllJOCmpnyCompensationItems()
        {
            var joCompanyCompensationA = joCompanyCompensation.Where(jo => jo.OptionNumber > 0).ToList();
            var IDs = joCompanyCompensationA.Select(jo => jo.Id).ToList();

            foreach (var compensation in joCompanyCompensationA)
            {
                compensation.CmpnyCmpnstnId = selectedCmpnyCmpnstnId;
            }

            //Reset All
            foreach (var joCmpnyCompensationItem in joCompanyCompensationItems
                .Where(jo=>IDs
                    .Contains(jo.JOCmpnyCmpnstnId.GetValueOrDefault())))
            {
                joCmpnyCompensationItem.MonthlyAmount = null;
                joCmpnyCompensationItem.AnnualAmount = null;
                joCmpnyCompensationItem.IsAnalysis = null;
                joCmpnyCompensationItem.IsEditable = null;
            }

            //Add Amount
            foreach (var vwCompanyCompensationItem in vwCompanyCompensationItems)
            {
                foreach (var joCmpnyCompensationItem in joCompanyCompensationItems
                    .Where(jo => IDs.Contains(jo.JOCmpnyCmpnstnId.GetValueOrDefault())
                        && jo.ItemId == vwCompanyCompensationItem.ItemId)
                )
                {
                    joCmpnyCompensationItem.MonthlyAmount = vwCompanyCompensationItem.MonthlyAmount;
                    joCmpnyCompensationItem.AnnualAmount = vwCompanyCompensationItem.AnnualAmount;
                    joCmpnyCompensationItem.IsAnalysis = vwCompanyCompensationItem.IsAnalysis;
                    joCmpnyCompensationItem.IsEditable = vwCompanyCompensationItem.IsEditable;
                }
            }

            ApplyProposedSalaryToBasicPayItems();
        }

        private List<string> CollectErrors(List<string> errors)
        {
            CollectJOCompanyCompensationErrors(errors);

            return errors;
        }

        private async Task SaveAnalysis()
        {
            var errors = CollectErrors(new List<string>());

            if (errors.Any())
            {
                await AlertService.Errors(errors);
                return;
            }

            if (!await AlertService.Confirm(
                title: "Save this analysis?",
                confirmText: "Save Analysis"))
            {
                return;
            }

            await CompensationService.SaveAnalysis(
                joCompanyCompensation,
                joCompanyCompensationItems,
                joAnalysis,
                jobOffer,
                selectedCmpnyCmpnstnId,
                candidate.Id,
                userId);

            await AlertService.Success("Analysis successfully saved.");
            //Navigation.Refresh();
        }

        private async Task SubmitForApproval()
        {
            var errors = CollectErrors(new List<string>());

            if (string.IsNullOrWhiteSpace(taPartnerRemarks))
            {
                errors.Add("TA Partner Remarks is required.");
            }

            if (errors.Any())
            {
                await AlertService.Errors(errors);
                return;
            }

            if (!await AlertService.Confirm(
                title: "Submit this analysis for approval?",
                confirmText: "Submit for Approval"))
            {
                return;
            }

            var submittedJobOfferId = await CompensationService.SubmitForApproval(
                jobOffer,
                joAnalysis,
                joCompanyCompensation,
                joCompanyCompensationItems,
                selectedCmpnyCmpnstnId,
                candidate.Id,
                userId,
                taPartnerRemarks);

            await AlertService.Success("Analysis successfully submitted for approval.");
            Navigation.NavigateTo($"{JORoutes.TA.JobOfferDetails}/{submittedJobOfferId}");
        }

        private void CollectJOCompanyCompensationErrors(List<string> errors)
        {
            var options = joCompanyCompensation
                .Where(compensation => compensation.OptionNumber > 0)
                .OrderBy(compensation => compensation.OptionNumber)
                .ToList();

            for (var index = 0; index < options.Count; index++)
            {
                var option = options[index];
                var optionNumber = option.OptionNumber.GetValueOrDefault();

                if (!option.ProposedSalary.HasValue)
                {
                    errors.Add($"Option {optionNumber}: Proposed Salary is required.");
                    continue;
                }

                if (!option.OfferRangeId.HasValue)
                {
                    errors.Add($"Option {optionNumber}: Proposed Salary is out of range.");
                    continue;
                }

                if (option.ProposedSalary.Value <= option.CurrentSalary.GetValueOrDefault())
                {
                    errors.Add($"Option {optionNumber}: Proposed Salary must be greater than Current Salary.");
                }

                if (index > 0)
                {
                    var previousOption = options[index - 1];

                    if (previousOption.ProposedSalary.HasValue
                        && option.ProposedSalary.Value <= previousOption.ProposedSalary.Value)
                    {
                        errors.Add(
                            $"Option {optionNumber}: Proposed Salary must be greater than Option {previousOption.OptionNumber.GetValueOrDefault()} Proposed Salary.");
                    }
                }
            }
        }

        private void ComputeIncrease(JOCompanyCompensation compensation)
        {
            decimal proposedSalary = compensation.ProposedSalary.GetValueOrDefault();
            decimal currentSalary = compensation.CurrentSalary.GetValueOrDefault();

            compensation.Increase = currentSalary == 0
                ? 0
                : Math.Round(((proposedSalary - currentSalary) / currentSalary) * 100m, 2);

            SetBandStatus(compensation);

            ApplyProposedSalaryToBasicPayItems();

            decimal current = joCompanyCompensationItems
                .Where(jo => jo.JOCmpnyCmpnstnId == currentJOCmpnyCmpnstnId)
                .Sum(jo => jo.MonthlyAmount.GetValueOrDefault());

            decimal proposal = joCompanyCompensationItems
                .Where(jo => jo.JOCmpnyCmpnstnId == compensation.Id)
                .Sum(jo => jo.MonthlyAmount.GetValueOrDefault());

            compensation.DiffTotalMonthly = current == 0
                ? 0
                : Math.Round(((proposal - current) / current) * 100m, 2);

            decimal currentAnnual = joCompanyCompensationItems
                .Where(jo => jo.JOCmpnyCmpnstnId == currentJOCmpnyCmpnstnId)
                .Sum(jo => jo.AnnualAmount.GetValueOrDefault());

            decimal proposalAnnual = joCompanyCompensationItems
                .Where(jo => jo.JOCmpnyCmpnstnId == compensation.Id)
                .Sum(jo => jo.AnnualAmount.GetValueOrDefault());

            compensation.DiffTotalAnnually = currentAnnual == 0
                ? 0
                : Math.Round(((proposalAnnual - currentAnnual) / currentAnnual) * 100m, 2);
        }

        private void SetBandStatus(JOCompanyCompensation compensation)
        {
            if (!vwSalaryBand.Maximum.HasValue || !vwSalaryBand.CompaRatio.HasValue)
            {
                compensation.BandStatus = null;
                return;
            }

            decimal proposedSalary = compensation.ProposedSalary.GetValueOrDefault();
            decimal minimum = vwSalaryBand.Minimum.GetValueOrDefault();
            decimal midpoint = vwSalaryBand.Midpoint.GetValueOrDefault();
            decimal maximum = vwSalaryBand.Maximum.GetValueOrDefault();
            decimal compaRatio = vwSalaryBand.CompaRatio.Value;

            if (proposedSalary >= minimum && proposedSalary <= midpoint)
            {
                compensation.BandStatus = "Min to Mid";
                compensation.OfferRangeId = 1;
                compensation.Escalate = false;
            }
            else if (proposedSalary > midpoint && proposedSalary <= maximum)
            {
                compensation.BandStatus = "Mid to Max";
                compensation.OfferRangeId = 2;
                compensation.Escalate = false;
            }
            else if (proposedSalary > maximum && proposedSalary <= compaRatio)
            {
                compensation.BandStatus = "Above Max";
                compensation.OfferRangeId = 3;
                compensation.Escalate = true;
            }
            else
            {
                compensation.BandStatus = "Beyond Salary Grade";
                compensation.OfferRangeId = null;
                compensation.Escalate = null;
            }
        }

        private void ApplyProposedSalaryToBasicPayItems()
        {
            foreach (var compensation in joCompanyCompensation.Where(jo => jo.OptionNumber > 0))
            {
                if (!compensation.ProposedSalary.HasValue)
                    continue;

                decimal proposedSalary = compensation.ProposedSalary.Value;

                //Basic Pay
                foreach (var basicPayItem in joCompanyCompensationItems.Where(jo =>
                    jo.JOCmpnyCmpnstnId == compensation.Id && jo.ItemId == 1))
                {
                    basicPayItem.MonthlyAmount = proposedSalary;
                    basicPayItem.AnnualAmount = proposedSalary * 12m;
                }

                //13th Month Pay
                foreach (var thirteenthMonthPayItem in joCompanyCompensationItems.Where(jo =>
                    jo.JOCmpnyCmpnstnId == compensation.Id && jo.ItemId == 2))
                {
                    thirteenthMonthPayItem.AnnualAmount = proposedSalary;
                }

                //Bayanihan Bonus
                foreach (var bayanihanBonusItem in joCompanyCompensationItems.Where(jo =>
                    jo.JOCmpnyCmpnstnId == compensation.Id &&
                    jo.ItemId == 4 &&
                    jo.IsAnalysis == true))
                {
                    bayanihanBonusItem.AnnualAmount = proposedSalary * 2m;
                }

                //Performance Bonus
                foreach (var performanceBonusItem in joCompanyCompensationItems.Where(jo =>
                    jo.JOCmpnyCmpnstnId == compensation.Id &&
                    jo.ItemId == 13 &&
                    jo.IsAnalysis == true))
                {
                    performanceBonusItem.AnnualAmount = proposedSalary * 2m;
                }
            }
        }

        private static bool CanEditAnalysisItem(JOCompanyCompensationItems? item)
        {
            return item?.IsAnalysis == true && item.IsEditable == true;
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

        private async Task ClickGoBack()
        {
            if (!await AlertService.Confirm(
                title: "Go back without saving?",
                confirmText: "Yes"))
            {
                return;
            }

            if(jobOffer.WorkFlowId == 1 || jobOffer.WorkFlowId == null)
            {
                Navigation.NavigateTo(JORoutes.TA.Candidates);
            }
            else
            {
                Navigation.NavigateTo(JORoutes.TA.JobOfferTracker);
            }
        }
    }
}
