using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ISalaryMatrixService
    {
        Task<int> CreateMatrix(SalaryMatrix matrix, List<SalaryMatrixBand> salaryBands);
        Task<VwSalaryMatrix> GetMatrix(int matrixId);
        Task<List<VwSalaryMatrixBand>> GetSalaryBands(int matrixId);
    }
}