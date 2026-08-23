using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.View;

namespace JO.Service.Services.Contracts
{
    public interface IManageUsersService
    {
        Task<int> CreateJobOfferUser(JobOfferUsers newUser, List<string> roleNames);
        Task<int> DeactivateUser(int userId);
        Task<JobOfferUsers?> GetActiveUserByEmail(string email);
        Task<List<JobOfferUsers>> GetAllUsersAsync();
        Task<List<VwUserDivisionAccess>> GetDivisionAccessByUserId(int id);
        Task<List<VwJOUserRolesDto>> GetJOUserRolesById(int id);
        Task<List<string?>> GetJOUserSelectedRolesById(int id);
        Task<List<string?>> GetRoles();
        Task<JobOfferUsers?> GetUserById(int id);
        Task<List<UserPermissionsDto>> GetUserPermissions();
        Task<List<UserPermissionsDto>> GetUserPermissions(int userId);
        Task<List<VwDivisions>> GetVwDivisionsByIds(List<int> divisionIds);
        Task<List<VwDivisions>> GetVwDivisionsByUserId(int userId);
        Task<List<VwJobOfferUsers>> GetVwJobOfferUsers();
        Task<List<VwRolePermissions>> GetVwRolePermissions();
        Task<List<VwJOUserAspNetRoles>> GetVwJOUserAspNetRoles();
        Task<bool> IsUserExists(string email, int userId = 0);
        Task<int> NewUserAsync(string name, string email, List<string> roles, List<int> divisionIds, List<int> permissionIds);
        Task<List<VwDivisions>> SearchVwDivisions(int companyId, string keyword);
        Task<int> ToggleIsActive(int userId);
        Task<int> UpdateJobOfferUser(JobOfferUsers newUser, List<string> roleNames);
        Task<int> UpdateUserARBAC(int userId, List<string> roles, List<int> divisionIds, List<int> permissionIds);
        Task<int> UpdateUserAsync(int userId, bool isActive, string name, string email, List<string> roles);
        Task<JobOfferUsers> GetJobOfferUser(int joUserId);
        Task<List<string>> GetJobOfferUserRoles(JobOfferUsers joUser);
        Task<List<VwJobOfferUsersAndRoles>> GetVwJobOfferUsersAndRoles(int joUserId);
    }
}
