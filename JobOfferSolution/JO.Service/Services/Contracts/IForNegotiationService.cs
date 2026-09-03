using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IForNegotiationService
    {
        Task<int> SaveAnalysis(List<JOCompanyCompensation> joCompanyCompensation, List<JOCompanyCompensationItems> joCompanyCompensationItems, JOAnalysis joAnalysis, JobOffers jobOffer, int userId);
        Task<int> SubmitForApproval(JobOffers jobOffer, JOAnalysis joAnalysis, List<JOCompanyCompensation> joCompanyCompensation, List<JOCompanyCompensationItems> joCompanyCompensationItems, int userId, string taPartnerRemarks);
    }
}