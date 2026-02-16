using JO.DataModel.Entity;

namespace JO.Service.Services.Contracts
{
    public interface IManageUsersService
    {
        List<string> GetAllRoles();
        Task<List<JobOfferUsers>> GetAllUsersAsync();
        Task<IEnumerable<string>> GetUserRoles(string aspNetUserId);
        Task<int> NewUserAsync(string name, string email, List<string> roles);
        Task<int> UpdateUserAsync(int userId, bool isActive, string name, string email, List<string> roles);
    }
}