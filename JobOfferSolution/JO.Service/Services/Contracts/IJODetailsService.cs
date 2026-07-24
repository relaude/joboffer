using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJODetailsService
    {
        Task<Candidates> GetCandidate(int id);
        Task<List<GroupApprovalsDto>> GetGroupApprovals(int jobOfferId);
        Task<JobOffers> GetJobOffer(int id);
        Task<List<VwJobOffers>> GetJobOffers();
        Task<List<VwJobOfferWorkFlow>> GetJobOfferWorkFlow(int jobOfferId);
        Task<VwLegalEntities> GetLegalEntity(int jobOfferId);
        Task<List<Proposal>> GetProposal(int jobOfferId);
        Task<Requests> GetRequest(int id);
        Task<List<SalaryBandStatus>> GetSalaryBandStatus();
        Task<List<VwApprovals>> GetVwApprovals(int jobOfferId);
        List<JOTabs> SetTabs(List<VwJobOfferWorkFlow> workFlow);
    }
}