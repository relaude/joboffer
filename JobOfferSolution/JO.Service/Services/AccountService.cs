using JO.DataModel.Identity;
using JO.Persistence.DataAccess;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDbContextFactory<JobOfferDbContext> _contextFactory;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDbContextFactory<JobOfferDbContext> contextFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _contextFactory = contextFactory;
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
                .PasswordSignInAsync(
                    user,
                    CommonConstant.DefaultPassword,
                    isPersistent: true,
                    lockoutOnFailure: false);

            return passwordSignInResult.Succeeded;
        }

        public async Task LocalLogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
