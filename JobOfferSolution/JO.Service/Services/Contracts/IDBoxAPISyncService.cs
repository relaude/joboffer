using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IDBoxAPISyncService
    {
        Task<List<DboxCandidatesRawData>> GetDboxCandidatesRawData();
        Task<List<DboxCandidatesRawData>> SaveDboxCandidatesRawData(Stream source, int createdBy);
    }
}
