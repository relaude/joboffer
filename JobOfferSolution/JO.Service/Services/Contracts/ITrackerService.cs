using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface ITrackerService
    {
        Task<List<VwJobOffers>> GetJobOffers();
    }
}