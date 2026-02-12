using JO.DataModel.Entity;
using JO.DataModel.Identity;
using JO.Persistence.Repositories.Contracts;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JO.Service.Services
{
    public class ManageUsersService : IManageUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJobOfferUsersRepo _joUserRepo;
        public ManageUsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJobOfferUsersRepo joUserRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _joUserRepo = joUserRepo;
        }

        public async Task<int> NewUserAsync(
            string name, 
            string email, 
            List<string> roles)
        {
            foreach (var role in roles)
                await CreateRole(role);

            string aspnetId = await Register(email);

            await UpdateRoles(aspnetId, roles);

            var newUser = new JobOfferUsers
            {
                AspNetUser_Id = aspnetId,
                Name = name,
                Email = email,
                IsActive = true
            };

            await _joUserRepo.AddAsync(newUser);
            await _joUserRepo.SaveChangesAsync();

            await AddClaims(aspnetId, newUser.Id);

            return newUser.Id;
        }

        public async Task<int> UpdateUserAsync(
            int userId,
            bool isActive,
            string name,
            string email,
            List<string> roles)
        {
            var joUser = await _joUserRepo.GetByIdAsync(userId);
            if (isActive)
            {
                await UpdateRoles(joUser.AspNetUser_Id, roles);
                await UpdateAspName(joUser.AspNetUser_Id, email);
                joUser.Name = name;
                joUser.Email = email;
            }

            joUser.IsActive = isActive;
            
            _joUserRepo.Update(joUser);
            return await _joUserRepo.SaveChangesAsync();
        }

        public List<string> GetAllRoles()
        {
            var roles = new List<string>();
            roles.AddRange([JOUSerRole.Admin,
                JOUSerRole.TA,
                JOUSerRole.HRBP,
                JOUSerRole.HROD,
                JOUSerRole.DH]);

            return roles;
        }

        public async Task<IEnumerable<JobOfferUsers>> GetAllUsersAsync()
        {
            return await _joUserRepo.GetAllAsync();
        }

        public async Task<IEnumerable<string>> GetUserRoles(string aspNetUserId)
        {
            var user = await _userManager.FindByIdAsync(aspNetUserId);
            return await _userManager.GetRolesAsync(user);
        }

        private async Task<bool> CreateRole(string roleName)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleName);
            if (existingRole != null)
            {
                return true;
            }

            var newRole = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(newRole);

            return result.Succeeded;
        }

        private async Task<string> Register(string email)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var newUser = await _userManager.CreateAsync(user, CommonConstant.DefaultPassword);

            if (newUser.Succeeded)
            {
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailConfirm = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
            }

            return user.Id;
        }

        private async Task UpdateRoles(string aspNetUserId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(aspNetUserId);
            if (user == null)
                return;

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (roles != null && roles.Any())
                await _userManager.AddToRolesAsync(user, roles);
        }

        private async Task AddClaims(string aspNetUserId, int userId)
        {
            var user = await _userManager.FindByIdAsync(aspNetUserId);
            var claims = new List<Claim>
            {
                new("JobOfferUserId", userId.ToString())
            };

            await _userManager.AddClaimsAsync(user, claims);
        }

        private async Task UpdateAspName(string aspNetUserId, string email)
        {
            var user = await _userManager.FindByIdAsync(aspNetUserId);
            user.Email = email;
            user.UserName = email;

            await _userManager.UpdateAsync(user);
        }
    }
}
