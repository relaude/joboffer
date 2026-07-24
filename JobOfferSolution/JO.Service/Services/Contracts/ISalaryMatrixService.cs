using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ISalaryMatrixService
    {
        Task<int> CreateMatrix(SalaryMatrix matrix, List<SalaryMatrixBand> salaryBands);
        Task<VwSalaryMatrix> GetMatrix(int matrixId);
        Task<List<VwSalaryMatrix>> GetMatrixList();
        Task<List<VwSalaryMatrixBand>> GetSalaryBands(int matrixId);
        Task<List<VwSalaryMatrixBand>> GetSalaryBandsByJOId(int jobOfferId);
        Task<int> UpdateMatrixEffectiveDate(int matrixId, DateTime effectiveTo, bool isActive, int modifiedBy);
    }
}