using JO.DataModel.DTOs;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<IEnumerable<VwCandidates>> GetAllCandidates();
        Task<VwCandidates> GetCandidate(int id);
        Task<int> NewTransactionAsync(string name, string email, List<AttachmentDto> attachments);
    }
}