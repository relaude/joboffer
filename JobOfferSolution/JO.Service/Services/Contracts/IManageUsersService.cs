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
        Task<IEnumerable<string>> GetUserRoles(string aspNetUserId);
        Task<List<VwJOUserRoles>> GetVwJOUserRoles();
        Task<bool> IsUserExists(string email, int userId = 0);
        Task<int> NewUserAsync(string name, string email, List<string> roles);
        Task<int> UpdateUserAsync(int userId, bool isActive, string name, string email, List<string> roles);
    }
}