using KalanalyzeCode.ConfigurationManager.Ui.Models;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class IdentityServerExtension
{
    public static WebApplicationBuilder AddIdentityServerFunction(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>();

        return builder;
    }
    
    public static WebApplication UseIdentityServerFunction(this WebApplication app)
    {
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.UseAntiforgery();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}