using KalanalyzeCode.ConfigurationManager.Ui.Client.Authorization;
using KalanalyzeCode.ConfigurationManager.Ui.Data;
using KalanalyzeCode.ConfigurationManager.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class IdentityServiceExtension
{
    public static WebApplicationBuilder AddIdentityService(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<PermissionClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();
        
        
        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return builder;
    }
}