using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IAccountService
    {
        Task CreateRole(string roleName);
        Task<int> GetJobOfferUserId();
        Task<List<VwJOUserRoles>> GetUserRolesAsync(string email);
        Task<bool> LocalLogIn(string email);
        Task LocalLogOut();
    }
}