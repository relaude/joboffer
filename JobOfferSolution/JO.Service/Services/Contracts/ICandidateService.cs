using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateService
    {
        Task<int> EmailRequest(Requests entity);
        Task<Candidates> GetCandidate(int id);
        Task<List<Candidates>> GetCandidates();
    }
}