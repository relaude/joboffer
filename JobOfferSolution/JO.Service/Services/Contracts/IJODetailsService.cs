using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJODetailsService
    {
        Task<Candidates> GetCandidate(int id);
        Task<List<CompensationPackage>> GetCompensationPackage(int jobOfferId);
        Task<List<VwDiscussions>> GetDiscussions(int jobOfferId);
        Task<List<VwJODboxCandidates>> GetDivHeadL1ForApprovalVwJODboxCandidates(int userId);
        Task<List<VwJODboxCandidates>> GetDivHeadL2ForApprovalVwJODboxCandidates(int userId);
        Task<List<VwJODboxCandidates>> GetHRODHeadForApprovalVwJODboxCandidates(int userId);
        Task<JobOffers> GetJobOffer(int id);
        Task<List<VwJobOffers>> GetJobOffers();
        Task<List<VwJobOfferWorkFlow>> GetJobOfferWorkFlow(int jobOfferId);
        Task<VwLegalEntities> GetLegalEntity(int jobOfferId);
        Task<List<VwJODboxCandidates>> GetPEHeadForReviewVwJODboxCandidates(int userId);
        Task<List<VwJODboxCandidates>> GetPresidentForApprovalVwJODboxCandidates(int userId);
        Task<List<Proposal>> GetProposal(int jobOfferId);
        Task<Requests> GetRequest(int id);
        Task<List<SalaryBandStatus>> GetSalaryBandStatus();
        Task<List<VwJODboxCandidates>> GetTALeadForReviewVwJODboxCandidates(int userId);
        Task<List<VwApprovals>> GetVwApprovals(int jobOfferId);
        Task<List<VwJODboxCandidates>> GetVwJODboxCandidates();
        Task<List<VwPckgTempHasItms>> GetVwPckgTempHasItms(int templateId);
        Task<List<VwTALeadDboxCandidates>> GetVwTALeadDboxCandidates();
        Task<List<VwTAPartnerDboxCandidates>> GetVwTAPartnerDboxCandidates();
        List<JOTabs> SetNewOfferTabs();
        List<JOTabs> SetTabs(List<VwJobOfferWorkFlow> workFlow);
        List<JOTabs> SetTabs();
    }
}