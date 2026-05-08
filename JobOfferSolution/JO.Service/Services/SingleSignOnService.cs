using JO.DataModel.Identity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JO.Service.Services
{
    public class SingleSignOnService : ISingleSignOnService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IManageUsersService _manageUsersService;

        public SingleSignOnService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IManageUsersService manageUsersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _manageUsersService = manageUsersService;
        }

        public async Task OnTokenValidatedAsync(TokenValidatedContext context)
        {
            var principal = context.Principal;

            if (principal?.Identity is not ClaimsIdentity claimsIdentity)
                return;

            var email = principal.FindFirstValue("preferred_username")
                ?? principal.FindFirstValue(ClaimTypes.Email)
                ?? principal.Identity?.Name;

            if (string.IsNullOrWhiteSpace(email))
                return;

            var joUser = await _manageUsersService.GetActiveUserByEmail(email);

            if (joUser is null)
                return;

            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser is null)
                return;

            if (!await _signInManager.CanSignInAsync(identityUser))
                return;

            var roles = await _userManager.GetRolesAsync(identityUser);

            foreach (var role in roles)
            {
                if (await _roleManager.RoleExistsAsync(role)
                    && !claimsIdentity.HasClaim(ClaimTypes.Role, role))
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
            }

            if (!claimsIdentity.HasClaim(claim => claim.Type == "JobOfferUserId"))
            {
                claimsIdentity.AddClaim(
                    new Claim("JobOfferUserId", joUser.Id.ToString()));
            }
        }
    }
}
