using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ICandidateDiscussionService
    {
        Task<VwJobOfferAnalysis> GetJobOfferAnalysis(int applicationId);
    }
}