using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJobOfferService
    {
        Task<int> CreateJobOffer(int candidateId, int positionId, int departmentId, decimal basicSalary, decimal allowance, decimal signingBonus, DateTime offerDate, DateTime startDate, string remarks);
        Task<int> DeclineJobOffer(int id, int statusId, int reasonId, string otherReason);
        Task<VwJobOffers> GetJobOffer(int id);
        Task<(IEnumerable<VwJobOffers> Data, int TotalCount)> SearchJobOffersAsync(int statusId, string? candidate, int page, int pageSize);
        Task<int> SetJobOfferStatus(int id, int statusId);
    }
}