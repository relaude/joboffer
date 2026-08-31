using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<int> CreateJobOffer(int candidateId, int createdBy);
        Task<int> CreateJobOffer(VwDboxCandidates candidate, int options, int createdBy);
        Task<int> EmailRequest(Requests entity);
        Task<Candidates> GetCandidate(int id);
        Task<string> GetCandidateLink(VwDboxCandidates candidate);
        Task<CandidateResponses> GetCandidateResponse(int id);
        Task<List<CandidateResponses>> GetCandidateResponses();
        Task<List<Candidates>> GetCandidates();
        Task<List<DboxCandidates>> GetDboxCandidates();
        Task<string> GetTALeadCandidateLink(VwDboxCandidates candidate);
        Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId);
        Task<List<VwDboxCandidates>> GetVwDboxCandidates();
    }
}