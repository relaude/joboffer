using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IDivisionAccessService
    {
        Task<List<Companies>> GetCompanies();
        Task<List<Divisions>> GetDivisions(int companyId);
        Task<VwJobOfferUsers> GetVwJobOfferUser(int userId);
        Task<List<int>> InitSelectedDivisionIds(List<int> selectedDivisionIds, int joUserId);
        Task<int> UpdateUserDivisionAccess(List<int> selectedDivisionIds, int joUserId);
    }
}