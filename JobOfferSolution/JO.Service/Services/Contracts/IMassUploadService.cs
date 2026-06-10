using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IMassUploadService
    {
        Task<List<CandidateTempData>> GetCandidateTempData();
        Task<int> SaveExcelRowItems(Stream excelStream, int createdBy);
    }
}