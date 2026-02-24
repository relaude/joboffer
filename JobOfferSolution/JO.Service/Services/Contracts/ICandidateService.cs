using JO.DataModel.DTOs;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<IEnumerable<VwCandidates>> GetAllCandidates();
        Task<VwCandidates> GetCandidate(int id);
        Task<int> UpdatePersonalInfo(int id, string firstName, string lastName, string email, string contactNumber, bool isHrod);
    }
}