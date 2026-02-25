using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDashboardService
    {
        Task<IEnumerable<VwJobOffers>> GetAllJobOffers();
        Task<IEnumerable<VwJobOffers>> GetAllJobOffers(int statusId);
    }
}