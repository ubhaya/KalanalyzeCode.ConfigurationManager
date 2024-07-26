using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Ui.HttpHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extensions;

public static partial class Extensions
{
    public static IServiceCollection AddOpenIdConnect(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddTransient<AuthenticationDelegatingHandler>();

        services.AddAntiforgery(options => options.Cookie.Name = "ClientBlazorAppAntiForgeryCookie")
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "ClientBlazorAppAuthCookie";
                options.AccessDeniedPath = "/AccessDenied";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://localhost:5001/";
                options.ClientId = "KalanalyzeCode.ConfigurationManager.Ui";
                options.ClientSecret = "secret";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UseTokenLifetime = false;

                //options.SignedOutRedirectUri = "/";

                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.Scope.Add("KalanalyzeCode.ConfigurationManager");

                options.ClaimActions.MapJsonKey(CustomClaimTypes.Permissions, CustomClaimTypes.Permissions);

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
            });
        
        return services;
    }
}