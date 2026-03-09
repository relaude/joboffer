using JO.DataModel.DTOs;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<PagedResult<VwCandidates>> CandidatesForJOCretionAsync(string? candidate, int page, int pageSize);
        Task<IEnumerable<VwCandidates>> GetAllCandidates();
        Task<VwCandidates> GetCandidate(int id);
        Task<string> GetCandidateEmail(int id);
        Task<PagedResult<VwCandidates>> SearchCandidatesAsync(int statusId, string? candidate, int page, int pageSize);
        Task<int> UpdatePersonalInfo(int id, string firstName, string lastName, string email, string contactNumber, bool isHrod);
    }
}