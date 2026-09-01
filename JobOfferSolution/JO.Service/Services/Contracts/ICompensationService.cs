using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICompensationService
    {
        Task<int> CreateJobOffer(VwDboxCandidates candidate, int options, int createdBy);
        Task<List<CompanyCompensation>> GetCompanyCompensation(int companyId);
        Task<JOAnalysis> GetJOAnalysis(int jobOfferId);
        Task<JobOffers> GetJobOffer(int jobOfferId);
        Task<List<JOCompanyCompensationItems>> GetJOCmpnyCompensationItems(int jobOfferId);
        Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId);
        Task<List<VwCompanyCompensation>> GetVwCompanyCompensation();
        Task<List<VwCompanyCompensationItems>> GetVwCompanyCompensationItems(int compensationId);
        Task<VwJobOffers> GetVwJobOffer(int jobOfferId);
        Task<VwJODboxCandidates> GetVwJODboxCandidate(int jobOfferId);
        Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId);
        Task<VwSalaryBands> GetVwSalaryBand(int companyId, int csgId);
        Task<int> SaveAnalysis(List<JOCompanyCompensation> joCompanyCompensation, List<JOCompanyCompensationItems> joCompanyCompensationItems, JOAnalysis joAnalysis, JobOffers jobOffer, int selectedCmpnyCmpnstnId, int candidateId, int userId);
        Task<List<CompenItemCategoryDto>> SetUpCompenItemCategoryDto();
        Task<int> SubmitForApproval(JobOffers jobOffer, JOAnalysis joAnalysis, List<JOCompanyCompensation> joCompanyCompensation, List<JOCompanyCompensationItems> joCompanyCompensationItems, int selectedCmpnyCmpnstnId, int candidateId, int userId, string taPartnerRemarks);
        Task<int> SubmitForApprovalByTALead(JobOffers jobOffer, JOAnalysis joAnalysis, List<JOCompanyCompensation> joCompanyCompensation, List<JOCompanyCompensationItems> joCompanyCompensationItems, int selectedCmpnyCmpnstnId, int candidateId, int userId, string taPartnerRemarks);
        Task<int> UpdateCompensationItems(List<VwCompanyCompensationItems> compensationItems, int userId);
    }
}
