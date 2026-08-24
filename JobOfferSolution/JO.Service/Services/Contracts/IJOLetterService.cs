using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOLetterService
    {
        Task<CompanyCompensation> GetCompanyCompensation(int compensationId);
        Task<JobOffers> GetJobOffer(int jobOfferId);
        Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId);
        Task<List<JOItemLetter>> GetJOItemLetter(int compensationId);
        Task<List<VwCompanyCompensationItems>> GetVwCompanyCompensationItems(int compensationId);
        Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId);
        void UpdateItemLetterPlaceHolder(List<JOItemLetter> joItemLetter, VwDboxCandidates candidate, decimal proposedSalary);
    }
}