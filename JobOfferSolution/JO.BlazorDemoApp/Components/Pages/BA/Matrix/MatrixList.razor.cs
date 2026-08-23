using JO.DataModel.View;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace JO.BlazorDemoApp.Components.Pages.BA.Matrix
{
    public partial class MatrixList
    {
        [Inject] private ISalaryMatrixService SalaryMatrixService { get; set; } = default!;

        private List<VwSalaryMatrix> matrixList = new();

        protected override async Task OnInitializedAsync()
        {
            matrixList = await SalaryMatrixService.GetVwSalaryMatrix();
        }
    }
}
