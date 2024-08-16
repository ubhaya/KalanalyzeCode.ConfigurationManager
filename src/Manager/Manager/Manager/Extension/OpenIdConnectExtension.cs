using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class OpenIdConnectExtension
{
    public static IServiceCollection AddOpenIdConnect(this IServiceCollection services)
    {
        // services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        // services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        // services.AddTransient<AuthenticationDelegatingHandler>();

        services.AddAntiforgery(options => options.Cookie.Name = "ClientBlazorAppAntiForgeryCookie")
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = 
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "ClientBlazorAppAuthCookie";
                options.AccessDeniedPath = "/AccessDenied";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:7206/";
                options.ClientId = AppConstants.Identity.ClientId;
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UseTokenLifetime = false;

                options.SignedOutRedirectUri = "/";

                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.Scope.Add(AppConstants.Identity.ScopeName);

                //options.ClaimActions.MapJsonKey(CustomClaimTypes.Permissions, CustomClaimTypes.Permissions);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    NameClaimType = "name",
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:7206/";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };
            });

        return services;
    }

    public static WebApplication UseOpenIdConnectEndpoint(this WebApplication app)
    {
        app.MapPost("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        });

        app.MapGet("/login", async (string returnUrl, HttpContext context) =>
        {
            await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = returnUrl });
        });

        return app;
    }
}