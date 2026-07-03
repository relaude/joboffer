using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOAnalysisService
    {
        Task<VwCandidateApplications> GetCandidateApplication(int id);
        Task<List<VwCandidateApplications>> GetCandidateApplications();
        Task<List<JobOfferProposal>> InitializeProposal(int applicationId, int salaryMatrixId, int salaryMatrixBandId, decimal currentSalary, int createdBy);
        Task<int> LegalEntitySetup(CandidateApplications entity);
        Task<List<JobOfferProposal>> ReComputeProposalAnalysis(List<JobOfferProposal> proposals);
    }
}