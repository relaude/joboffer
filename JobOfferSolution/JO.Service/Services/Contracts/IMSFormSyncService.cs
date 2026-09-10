using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IMSFormSyncService
    {
        Task<bool> HasExcelDataRowsAsync(Stream source);
        Task<List<CandidateResponseRawData>> GetCandidateResponses(int createdBy);
        Task<DateTime?> GetLatestDateTimeMSFormAsync();
        Task<List<CandidateResponseRawData>> SaveCandidateResponseRawData(int createdBy);
        Task<List<CandidateResponseRawData>> SaveCandidateResponseRawData(Stream source, int createdBy);
    }
}
