using JO.DataModel.Identity;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JO.BlazorApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly IAccountService _accountService;
        public Account(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IResult> LogIn([FromBody] LoginDto dto)
        {
            try
            {
                return Results.Ok(await _accountService.LocalLogIn(dto.Email));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IResult> LogOut()
        {
            try
            {
                await _accountService.LocalLogOut();
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
