using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IMSFormSyncService
    {
        Task<List<CandidateResponseRawData>> GetCandidateResponses(int createdBy);
        Task<List<CandidateResponseRawData>> SaveCandidateResponseRawData(int createdBy);
    }
}