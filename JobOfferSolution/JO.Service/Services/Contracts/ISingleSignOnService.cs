using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace JO.Service.Services.Contracts
{
    public interface ISingleSignOnService
    {
        Task OnTokenValidatedAsync(TokenValidatedContext context);
    }
}