using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IJOAnalysisService
    {
        Task<int> LegalEntitySetup(CandidateApplications entity);
    }
}