using JO.DataModel.DTOs;
using JO.DataModel.Entity;
using JO.DataModel.Identity;
using JO.DataModel.View;
using JO.Persistence;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JO.Service.Services
{
    public class ManageUsersService : IManageUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;

        public ManageUsersService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contextFactory = contextFactory;
        }

        public async Task<int> ToggleIsActive(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var joUser = await context.JobOfferUsers.FindAsync(userId);

            joUser.IsActive = !joUser.IsActive.Value;
            context.JobOfferUsers.Update(joUser);

            return await context.SaveChangesAsync();
        }

        public async Task<List<VwUserDivisionAccess>> GetDivisionAccessByUserId(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwUserDivisionAccess
                .AsNoTracking()
                .Where(jo=>jo.JobOfferUserId == id)
                .ToListAsync();
        }

        public async Task<List<string?>> GetJOUserSelectedRolesById(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.VwJOUsersInRoles
                .AsNoTracking()
                .Where(jo => jo.Id == id)
                .Select(jo => jo.RoleName)
                .ToListAsync();
        }

        public async Task<List<VwJOUserRolesDto>> GetJOUserRolesById(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var joUserInRoles = await context.VwJOUsersInRoles
                .AsNoTracking()
                .Where(jo => jo.Id == id)
                .Select(jo => jo.AspNetRoleId)
                .ToHashSetAsync();

            var results = await context.VwJOUserRoles
                .AsNoTracking()
                .Select(role => new VwJOUserRolesDto
                {
                    Id = role.Id,
                    AspNetRoleId = role.AspNetRoleId,
                    RoleName = role.RoleName,
                    RoleCategory = role.RoleCategory,
                    OrderBy = role.OrderBy,
                    Checked = joUserInRoles.Contains(role.AspNetRoleId)
                })
                .OrderBy(jo => jo.OrderBy)
                .ToListAsync();

            return results;
        }

        public async Task<List<VwRolePermissions>> GetVwRolePermissions()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwRolePermissions.AsNoTracking().ToListAsync();
        }

        public async Task<List<UserPermissionsDto>> GetUserPermissions()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var permissions = await context.Permissions
                .AsNoTracking()
                .Select(permission => new UserPermissionsDto
                {
                    Id = permission.Id,
                    PermissionCode = permission.PermissionCode,
                    PermissionName = permission.PermissionName,
                    Allowed = false
                })
                .ToListAsync();

            return permissions;
        }

        public async Task<List<UserPermissionsDto>> GetUserPermissions(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var userPermissions = await context.UserPermissions
                .AsNoTracking()
                .Where(jo => jo.JobOfferUserId == userId)
                .ToListAsync();

            var allowedPermissionCodes = userPermissions
                .Select(rolePermission => rolePermission.PermissionId)
                .ToHashSet();

            var permissions = await context.Permissions
                .AsNoTracking()
                .Select(permission => new UserPermissionsDto
                {
                    Id = permission.Id,
                    PermissionCode = permission.PermissionCode,
                    PermissionName = permission.PermissionName,
                    Allowed = allowedPermissionCodes.Contains(permission.Id)
                })
                .ToListAsync();

            return permissions;
        }

        public async Task<List<VwDivisions>> GetVwDivisionsByUserId(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var divisionIds = await context.UserDivisionAccess
                .AsNoTracking()
                .Where(jo => jo.JobOfferUserId == userId)
                .Select(jo => jo.DivisionId)
                .ToHashSetAsync();

            return await context.VwDivisions
                .AsNoTracking()
                .Where(jo => divisionIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwDivisions>> GetVwDivisionsByIds(List<int> divisionIds)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwDivisions
                .AsNoTracking()
                .Where(jo => divisionIds.Contains(jo.Id))
                .ToListAsync();
        }

        public async Task<List<VwDivisions>> SearchVwDivisions(int companyId, string keyword)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var query = context.VwDivisions
                .AsNoTracking()
                .AsQueryable();

            if(companyId > 0)
                query = query.Where(jo => jo.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(jo =>
                    EF.Functions.Like(jo.DivisionCode ?? "", $"%{keyword}%") ||
                    EF.Functions.Like(jo.DivisionName ?? "", $"%{keyword}%"));
            }

            return await query.ToListAsync();
        }

        public async Task<List<string?>> GetRoles()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJOUserRoles
                .AsNoTracking()
                .OrderBy(jo => jo.OrderBy)
                .Select(jo => jo.RoleName)
                .ToListAsync();
        }

        public async Task<bool> IsUserExists(string email, int userId = 0)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferUsers
                .AsNoTracking()
                .AnyAsync(jo => jo.Email == email && (userId == 0 || jo.Id != userId));
        }

        public async Task<int> NewUserAsync(
            string name,
            string email,
            List<string> roles,
            List<int> divisionIds,
            List<int> permissionIds)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                //ASP Identity
                var existingAspUser = await _userManager.FindByEmailAsync(email);
                if (existingAspUser != null)
                    throw new Exception("User already exists.");

                foreach (var role in roles)
                    await EnsureRoleExists(role);

                var aspUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var createResult = await _userManager.CreateAsync(
                    aspUser,
                    CommonConstant.DefaultPassword);

                if (!createResult.Succeeded)
                    throw new Exception(string.Join(",", createResult.Errors.Select(e => e.Description)));

                await _userManager.AddToRolesAsync(aspUser, roles);

                await _userManager.AddClaimAsync(
                    aspUser,
                    new Claim("Email", email));

                //New JO User
                var joUser = new JobOfferUsers
                {
                    AspNetUserId = aspUser.Id,
                    Name = name,
                    Email = email,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await context.JobOfferUsers.AddAsync(joUser);
                await context.SaveChangesAsync();

                await _userManager.AddClaimAsync(
                    aspUser,
                    new Claim("JobOfferUserId", joUser.Id.ToString()));

                //Division access
                List<UserDivisionAccess> divisionsAcess = new(); 
                foreach(var id in divisionIds)
                {
                    divisionsAcess.Add(new UserDivisionAccess 
                    { 
                        JobOfferUserId = joUser.Id, 
                        DivisionId = id  
                    });
                }
                await context.UserDivisionAccess.AddRangeAsync(divisionsAcess);

                //Permissions
                List<UserPermissions> permissions = new();
                foreach (var id in permissionIds)
                {
                    permissions.Add(new UserPermissions
                    {
                        JobOfferUserId = joUser.Id,
                        PermissionId = id
                    });
                }
                await context.UserPermissions.AddRangeAsync(permissions);

                //save divisions access and permissions
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return joUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> UpdateUserARBAC(int userId, 
            List<string> roles,
            List<int> divisionIds,
            List<int> permissionIds)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            //Roles
            var joUser = await context.JobOfferUsers.FirstOrDefaultAsync(x => x.Id == userId);
            var aspUser = await _userManager.FindByIdAsync(joUser.AspNetUserId);

            //New division access
            List<UserDivisionAccess> divisionsAcess = new();
            foreach (var id in divisionIds)
            {
                divisionsAcess.Add(new UserDivisionAccess
                {
                    JobOfferUserId = joUser.Id,
                    DivisionId = id
                });
            }

            //For deletion division access
            var removeDivisionAccess = await context.UserDivisionAccess
                .Where(jo => jo.JobOfferUserId == userId)
                .ToListAsync();

            //New Permissions
            List<UserPermissions> permissions = new();
            foreach (var id in permissionIds)
            {
                permissions.Add(new UserPermissions
                {
                    JobOfferUserId = joUser.Id,
                    PermissionId = id
                });
            }

            //For deletion permission
            var removePermissions = await context.UserPermissions
                .Where(jo => jo.JobOfferUserId == userId)
                .ToListAsync();

            //TRANSACTIONS
            //Roles Trx
            var currentRoles = await _userManager.GetRolesAsync(aspUser);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(aspUser, currentRoles);

            if (roles != null && roles.Any())
                await _userManager.AddToRolesAsync(aspUser, roles);

            //Division Trx
            context.UserDivisionAccess.RemoveRange(removeDivisionAccess);
            await context.UserDivisionAccess.AddRangeAsync(divisionsAcess);

            //Permission Trx
            context.UserPermissions.RemoveRange(removePermissions);
            await context.UserPermissions.AddRangeAsync(permissions);

            //Rollout Division and Permission Transaction
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateUserAsync(
            int userId,
            bool isActive,
            string name,
            string email,
            List<string> roles)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var joUser = await context.JobOfferUsers
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (joUser == null)
                    throw new Exception("User not found.");

                var aspUser = await _userManager.FindByIdAsync(joUser.AspNetUserId);
                if (aspUser == null)
                    throw new Exception("ASP.NET Identity user not found.");

                aspUser.Email = email;
                aspUser.UserName = email;

                var updateResult = await _userManager.UpdateAsync(aspUser);
                if (!updateResult.Succeeded)
                    throw new Exception("Failed to update Identity user.");

                var currentRoles = await _userManager.GetRolesAsync(aspUser);
                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(aspUser, currentRoles);

                if (roles != null && roles.Any())
                    await _userManager.AddToRolesAsync(aspUser, roles);

                joUser.Name = name;
                joUser.Email = email;
                joUser.IsActive = isActive;

                context.JobOfferUsers.Update(joUser);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();

                return joUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> DeactivateUser(int userId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            var joUser = await context.JobOfferUsers
                    .FirstOrDefaultAsync(x => x.Id == userId);

            joUser.IsActive = false;
            context.JobOfferUsers.Update(joUser);

            return await context.SaveChangesAsync();
        }

        public async Task<List<JobOfferUsers>> GetAllUsersAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferUsers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JobOfferUsers?> GetActiveUserByEmail(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo=>jo.IsActive == true && jo.Email==email);
        }

        public async Task<JobOfferUsers?> GetUserById(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferUsers.FindAsync(id);
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
