using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJODetailsService
    {
        Task<Candidates> GetCandidate(int id);
        Task<JobOffers> GetJobOffer(int id);
        Task<List<VwJobOfferWorkFlow>> GetJobOfferWorkFlow(int jobOfferId);
        Task<VwLegalEntities> GetLegalEntity(int jobOfferId);
        Task<Requests> GetRequest(int id);
        List<JOTabs> SetTabs(List<VwJobOfferWorkFlow> workFlow);
    }
}