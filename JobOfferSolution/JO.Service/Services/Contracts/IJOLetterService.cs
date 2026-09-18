using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJOLetterService
    {
        Task<int> SaveDraftJobOfferHasEmail(JobOfferHasEmail jobOfferEmail);

        /// <summary>
        /// Returns the candidate template with #JORefNum, #CandidateName, #Position, #SalaryGrade,
        /// #Company, #Department, and #Division replaced in EmailSubject and EmailMessage.
        /// Subject values remain plain text; message values are HTML-encoded.
        /// Returns an empty template when the offer, candidate, or template is missing.
        /// Does not modify the saved template; unrecognized tokens are preserved.
        /// </summary>
        Task<CandidateEmailTemplate> EditedEmailTemplate(int jobOfferId, int templateId);
        Task<CompanyCompensation> GetCompanyCompensation(int compensationId);
        Task<JobOffers> GetJobOffer(int jobOfferId);
        Task<List<JOCompanyCompensation>> GetJOCompanyCompensation(int jobOfferId);
        Task<List<JOItemLetter>> GetJOItemLetter(int compensationId);
        Task<List<VwCompanyCompensationItems>> GetVwCompanyCompensationItems(int compensationId);
        Task<VwDboxCandidates> GetVwDboxCandidate(int candidateId);
        Task<VwJODboxCandidates> GetVwJODboxCandidates(int jobOfferId);
        void UpdateItemLetterPlaceHolder(List<JOItemLetter> joItemLetter, VwDboxCandidates candidate, decimal proposedSalary);
        Task<List<VwJobOfferHasEmail>> GetVwJobOfferHasEmail();
        Task<List<VwJobOfferHasEmail>> GetApproverJobOfferHasEmail();
        Task<List<JOHasEmailStatus>> GetJOHasEmailStatus();
        Task<JobOfferHasEmail> GetJobOfferHasEmail(int emailId);
        Task<int> UpdateJobOfferHasEmail(JobOfferHasEmail jobOfferEmail);
        Task<List<JobOffers>> GetJobOffersForDiscussion();
        Task<int> AddRangeJOHasEmailAttach(List<JOHasEmailAttach> emailAttach);
        Task<List<JOHasEmailAttach>> GetJOHasEmailAttach(int emailId);
        Task AproveJobOfferHasEmail(int emailId);
    }
}
