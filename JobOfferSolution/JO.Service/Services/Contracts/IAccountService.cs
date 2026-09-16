using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IAccountService
    {
        Task CreateRole(string roleName);
        Task<int> GetJobOfferUserId();
        Task<List<VwJOUserRoles>> GetUserRolesAsync(string email);
        /// <summary>
        /// Returns distinct, non-empty email addresses for users in any of the supplied Identity roles.
        /// </summary>
        Task<List<string>> GetUserEmailsByRoleNamesAsync(List<string> roleNames);
        Task<bool> LocalLogIn(string email);
        Task LocalLogOut();
    }
}
