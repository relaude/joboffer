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

        public async Task<List<VwJOUserRoles>> GetVwJOUserRoles()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJOUserRoles
                .AsNoTracking()
                .OrderBy(jo=>jo.OrderBy)
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
            List<string> roles)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
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

                await transaction.CommitAsync();

                return joUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        public async Task<IEnumerable<string>> GetUserRoles(string aspNetUserId)
        {
            var user = await _userManager.FindByIdAsync(aspNetUserId);
            return user == null
                ? Enumerable.Empty<string>()
                : await _userManager.GetRolesAsync(user);
        }

        public async Task<JobOfferUsers?> GetActiveUserByEmail(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            return await context.JobOfferUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo=>jo.IsActive == true && jo.Email==email);
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public List<string> GetAllRoles()
        {
            return new List<string>
            {
                JOUserRole.Admin,
                JOUserRole.TA,
                JOUserRole.TR,
                JOUserRole.HRBP,
                JOUserRole.HROD,
                JOUserRole.DH
            };
        }
    }
}
