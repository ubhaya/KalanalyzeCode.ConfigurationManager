using KalanalyzeCode.ConfigurationManager.Ui.Data;
using KalanalyzeCode.ConfigurationManager.Ui.Models;
using Microsoft.AspNetCore.Identity;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class IdentityServiceExtension
{
    public static WebApplicationBuilder AddIdentityService(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return builder;
    }
}