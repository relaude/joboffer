using JO.DataModel.Identity;
using JO.Persistence.DataAccess;
using JO.DataModel.View;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JO.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDbContextFactory<JobOfferDbContext> contextFactory,
            AuthenticationStateProvider authStateProvider,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextFactory = contextFactory;
            _authStateProvider = authStateProvider;
            _roleManager = roleManager;
        }

        public async Task<bool> LocalLogIn(string email)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var joUser = await context.JobOfferUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(jo=>jo.Email == email 
                    && jo.IsActive == true);

            if (joUser == null)
                return false;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;

            var passwordSignInResult = await _signInManager
                .CheckPasswordSignInAsync(
                    user,
                    CommonConstant.DefaultPassword,
                    lockoutOnFailure: false);

            if (!passwordSignInResult.Succeeded)
                return false;

            var claims = new[]
            {
                new Claim("JobOfferUserId", joUser.Id.ToString())
            };

            await _signInManager.SignInWithClaimsAsync(
                user,
                isPersistent: true,
                claims);

            return true;
        }

        public async Task LocalLogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<int> GetJobOfferUserId()
        {
            var user = await GetUserAsync();
            string userId = user.FindFirst("JobOfferUserId")?.Value;

            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }

        public async Task CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<List<VwJOUserRoles>> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new List<VwJOUserRoles>();

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<IdentityRole>();

            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                    roles.Add(role);
            }

            var roleIds = roles.Select(role => role.Id).ToList();
            if (roleIds.Count == 0)
                return new List<VwJOUserRoles>();

            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.VwJOUserRoles
                .AsNoTracking()
                .Where(role => role.AspNetRoleId != null && roleIds.Contains(role.AspNetRoleId))
                .OrderBy(role => role.OrderBy)
                .ToListAsync();
        }

        private async Task<ClaimsPrincipal> GetUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }
    }
}
