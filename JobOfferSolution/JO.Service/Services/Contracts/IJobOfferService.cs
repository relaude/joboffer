using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IJobOfferService
    {
        Task<int> CreateJobOffer(int candidateId, int positionId, int departmentId, decimal basicSalary, decimal allowance, decimal signingBonus, DateTime offerDate, DateTime startDate, string remarks);
        Task<VwJobOffers> GetJobOffer(int id);
        Task<int> SetJobOfferStatus(int id, int statusId);
    }
}