using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Common.Models;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
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
        builder.Services.AddScoped<IAuthorizationHandler, ProjectAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return builder;
    }
}