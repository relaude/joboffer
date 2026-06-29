using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IMSFormRequestService
    {
        Task<VwCandidateMSFormRequests> GetMSFormRequest(int id);
        Task<List<VwCandidateMSFormRequests>> GetMSFormRequests();
        Task<int> TagMSFormRequestAsSubmitted(int id);
    }
}