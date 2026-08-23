using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Matrix
{
    public partial class EditMatrix
    {
        [Parameter] public int matrixId { get; set; }

        [Inject] private IDropDownListService DropDownListService { get; set; } = default!;
        [Inject] private IAccountService AccountService { get; set; } = default!;
        [Inject] private IAlertService AlertService { get; set; } = default!;
        [Inject] private ISalaryMatrixService SalaryMatrixService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private SalaryMatrix salaryMatrix = new();
        private VwSalaryMatrix vwSalaryMatrix = new();

        private List<DropdownDto> companies = new();
        private List<VwCompanySalaryGrades> salaryGrades = new();
        private List<SalaryBandsDto> salaryBands = new();
        private List<VwSalaryBands> vwSalaryBands = new();

        protected override async Task OnInitializedAsync()
        {
            salaryMatrix = await SalaryMatrixService.GetSalaryMatrix(matrixId);
            vwSalaryMatrix = await SalaryMatrixService.GetVwSalaryMatrix(matrixId);

            salaryGrades = await SalaryMatrixService
                    .GetCompanySalaryGrades(salaryMatrix.CompanyId.GetValueOrDefault());

            await FillSalaryBands();
        }

        private async Task FillSalaryBands()
        {
            vwSalaryBands = await SalaryMatrixService.GetVwSalaryBands(matrixId);

            foreach (var item in vwSalaryBands)
            {
                salaryBands.Add(new SalaryBandsDto
                {
                    CSGId = item.CSGId,
                    GradeName = item.GradeName,
                    LockCSGId = true,
                    Maximum = item.Maximum,
                    Midpoint = item.Midpoint,
                    Minimum = item.Minimum,
                    CompaRatio = item.CompaRatio,
                    TypeName = item.TypeName
                });
            }
        }

        private async Task SaveMatrix()
        {
            var errors = CollectErrors(new List<string>());

            if (errors.Any())
            {
                await AlertService.Errors(errors);
                return;
            }

            if (!await AlertService.Confirm()) return;

            salaryMatrix.ModifiedBy = await AccountService.GetJobOfferUserId();
            salaryMatrix.ModifiedAt = DateTime.Now;

            await SalaryMatrixService.UpdateMatrix(salaryMatrix, salaryBands);
            Navigation.NavigateTo($"{JORoutes.BA.SalaryMatrixDetails}/{matrixId}");
        }

        private List<string> CollectErrors(List<string> errors)
        {
            if (!salaryBands.Any())
                errors.Add("Please add at least one salary band.");
            else
                CollectSalaryBandErrors(errors);

            return errors;
        }

        private void CollectSalaryBandErrors(List<string> errors)
        {
            var csgIds = new HashSet<int>();

            for (var index = 0; index < salaryBands.Count; index++)
            {
                var band = salaryBands[index];
                var rowNumber = index + 1;

                if (!band.CSGId.HasValue || band.CSGId <= 0)
                    errors.Add($"Salary Band Row {rowNumber}: Salary Grade is required.");
                else if (!csgIds.Add(band.CSGId.Value))
                    errors.Add($"Salary Band Row {rowNumber}: Salary Grade is duplicated.");

                if (!band.Minimum.HasValue || band.Minimum <= 0)
                    errors.Add($"Salary Band Row {rowNumber}: Minimum amount is required.");
                if (!band.Midpoint.HasValue || band.Midpoint <= 0)
                    errors.Add($"Salary Band Row {rowNumber}: Midpoint amount is required.");
                if (!band.Maximum.HasValue || band.Maximum <= 0)
                    errors.Add($"Salary Band Row {rowNumber}: Maximum amount is required.");
                if (!band.CompaRatio.HasValue || band.CompaRatio <= 0)
                    errors.Add($"Salary Band Row {rowNumber}: CompaRatio amount is required.");

                if (band.Minimum.HasValue
                    && band.Midpoint.HasValue
                    && band.Maximum.HasValue
                    && band.Maximum > 0
                    && band.Midpoint > 0
                    && band.Maximum > 0
                    && band.CompaRatio > 0
                    && !(band.Minimum < band.Midpoint
                        && band.Midpoint < band.Maximum
                        && band.Maximum < band.CompaRatio))
                {
                    errors.Add($"Salary Band Row {rowNumber}: Minimum, Midpoint, Maximum and CompaRatio must be in ascending order.");
                }
            }
        }

        private void AddBand()
        {
            var assignedGradeIds = salaryBands
                .Where(band => band.CSGId.HasValue)
                .Select(band => band.CSGId.Value)
                .ToHashSet();

            var nextSalaryGrade = salaryGrades
                .FirstOrDefault(grade => !assignedGradeIds.Contains(grade.Id));

            if (nextSalaryGrade is null)
                return;

            salaryBands.Add(new SalaryBandsDto
            {
                CSGId = nextSalaryGrade.Id,
                Minimum = 30000,
                Midpoint = 50000,
                Maximum = 70000,
                CompaRatio = 90000
            });
        }

        private void RemoveBand(SalaryBandsDto band)
        {
            salaryBands.Remove(band);
        }
    }
}
