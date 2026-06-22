using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IManageUsersService
    {
        Task<int> DeactivateUser(int userId);
        Task<JobOfferUsers?> GetActiveUserByEmail(string email);
        List<string> GetAllRoles();
        Task<List<JobOfferUsers>> GetAllUsersAsync();
        Task<List<string?>> GetRoles();
        Task<IEnumerable<string>> GetUserRoles(string aspNetUserId);
        Task<List<VwDivisions>> GetVwDivisionsByIds(List<int> divisionIds);
        Task<List<VwJOUserRoles>> GetVwJOUserRoles();
        Task<bool> IsUserExists(string email, int userId = 0);
        Task<int> NewUserAsync(string name, string email, List<string> roles);
        Task<List<VwDivisions>> SearchVwDivisions(int companyId, string keyword);
        Task<int> UpdateUserAsync(int userId, bool isActive, string name, string email, List<string> roles);
    }
}