using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDiscussionService
    {
        Task ForNegotiation(JobOffers jobOffer, JOAnalysis joAnalysis, VwDboxCandidates candidate, List<JOCompanyCompensation> joCompanyCompensation, int options, int createdBy);
        Task<List<Proposal>> GetApprovedProposal(int jobOfferId);
        Task<List<CandResponse>> GetCandResponse();
        Task<List<ChannelTypes>> GetChannelTypes();
        Task<List<VwDiscussions>> GetDiscussions(int jobOfferId);
        Task<List<DiscussionStatus>> GetDiscussionStatus();
        Task<List<DiscussSteps>> GetDiscussSteps();
        Task<JobOffers> GetJobOffer(int jobOfferId);
        Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId);
        Task<List<JODeclineReason>> GetJODeclineReason();
        Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId);
        Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId);
        Task<int> SaveDiscussion(Discussions discussion);
        Task<int> SaveDiscussion(DiscussionDto dto);
        Task<int> TagAsAccepted(int jobOfferId);
    }
}