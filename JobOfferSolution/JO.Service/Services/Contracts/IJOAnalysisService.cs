using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOAnalysisService
    {
        Task<VwCandidateApplications> GetCandidateApplication(int id);
        Task<List<VwCandidateApplications>> GetCandidateApplications();
        Task<int> LegalEntitySetup(CandidateApplications entity);
    }
}