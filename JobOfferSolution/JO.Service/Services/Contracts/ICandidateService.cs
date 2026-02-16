using JO.DataModel.DTOs;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<int> NewTransactionAsync(string name, string email, List<AttachmentDto> attachments);
    }
}