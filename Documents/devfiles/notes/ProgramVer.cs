using CurrieTechnologies.Razor.SweetAlert2;
using JO.BlazorApp.Components;
using JO.BlazorApp.Components.Account;
using JO.BlazorApp.Data;
using JO.DataModel.Identity;
using JO.Persistence;
using JO.Service;
using JO.Service.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using JO.Service.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Razor Components
// -----------------------------
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// -----------------------------
// Database
// -----------------------------
builder.Services.AddSingleton<IAppSettings, AppSettings>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var appSettings = sp.GetRequiredService<IAppSettings>();
    var configuration = sp.GetRequiredService<IConfiguration>();

    var connectionStringName = appSettings.GetConnectionStringName();
    var connectionString = configuration.GetConnectionString(connectionStringName)
        ?? throw new InvalidOperationException(
            $"Connection string '{connectionStringName}' not found.");

    options.UseSqlServer(connectionString);
});

// -----------------------------
// ASP.NET Identity
// -----------------------------
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// -----------------------------
// Azure AD Login (Inline Configuration)
// -----------------------------
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

}).AddMicrosoftIdentityWebApp(options => {
    options.Instance = "https://login.microsoftonline.com/";

    options.ClientId = "be102ea6-07c9-42c6-a7fe-35475f2fcbb1";
    options.TenantId = "197912d9-ef46-4ad0-8b1d-71d9fc5d1ac9";

    options.CallbackPath = "/signin-oidc";

    options.Events ??= new OpenIdConnectEvents();

    options.Events.OnSignedOutCallbackRedirect = context =>
    {
        context.Response.Redirect("/");
        context.HandleResponse();
        return Task.CompletedTask;
    };

    options.Events.OnTokenValidated = async context =>
    {
        var ssoService = context.HttpContext.RequestServices
        .GetRequiredService<ISingleSignOnService>();

        await ssoService.OnTokenValidatedAsync(context);
    };

});

builder.Services.Configure<CookieAuthenticationOptions>(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.AccessDeniedPath = JORoutes.Public.Denied;
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = JORoutes.Public.Denied;
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

// -----------------------------
// Custom Services
// -----------------------------
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddJobOfferServices(builder.Configuration);


// -----------------------------
// 3rd Party Components
// -----------------------------
builder.Services.AddSweetAlert2();

var app = builder.Build();


// -----------------------------
// Pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();
app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
