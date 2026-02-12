using JO.DataModel.Identity;
using JO.Persistence.Repositories.Contracts;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJobOfferUsersRepo _joUserRepo;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJobOfferUsersRepo joUserRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _joUserRepo = joUserRepo;
        }

        public async Task<bool> LocalLogIn(string email)
        {
            var joUser = await _joUserRepo
                .FirstOrDefaultAsync(
                    x => x.Email == email 
                    && x.IsActive == true);
            
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
