using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Matrix
{
    public partial class MatrixDetails
    {
        [Parameter] public int matrixId { get; set; }

        [Inject] private ISalaryMatrixService SalaryMatrixService { get; set; } = default!;
        [Inject] private IUtilitiesService UtilitiesService { get; set; } = default!;

        private VwSalaryMatrix matrix = new();
        private List<VwSalaryBands> salaryBands = new();

        protected override async Task OnInitializedAsync()
        {
            matrix = await SalaryMatrixService.GetVwSalaryMatrix(matrixId);
            salaryBands = await SalaryMatrixService.GetVwSalaryBands(matrixId);
        }
    }
}
