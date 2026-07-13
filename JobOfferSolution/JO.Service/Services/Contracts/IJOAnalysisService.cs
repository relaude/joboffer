using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOAnalysisService
    {
        Task<VwCandidateApplications> GetCandidateApplication(int id);
        Task<List<VwCandidateApplications>> GetCandidateApplications();
        Task<List<VwCompensationBenefits>> GetCompensationBenefits(int packageId);
        Task<VwJobOfferAnalysis> GetJobOfferAnalysis(int id);
        Task<List<VwJobOfferAnalysis>> GetJobOfferAnalysis();
        Task<List<VwJobOfferProposal>> GetJobOfferProposal(int analysisId);
        Task<List<ValidationStatus>> GetValidationStatus();
        Task<List<JobOfferProposal>> InitializeProposal(int applicationId, int salaryMatrixId, int salaryMatrixBandId, decimal currentSalary, int createdBy, List<VwCompensationBenefits> compensationItems);
        Task<int> LegalEntitySetup(CandidateApplications entity);
        Task<List<JobOfferProposal>> ReComputeProposalAnalysis(List<JobOfferProposal> proposals, List<VwCompensationBenefits> compensationItems);
        Task<int> SaveProposalsAndAnalysis(List<JobOfferProposal> proposals, int selectedOptionNumber, int selectedPackageId, decimal expectedSalary, string analysisNotes);
    }
}