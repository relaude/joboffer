using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDBoxCandidateService
    {
        Task<List<VwDboxCandidates>> GetVwDboxCandidates();
        Task MergeCandidateAndResponse();
        Task MergeCandidateRawResponses();
    }
}
