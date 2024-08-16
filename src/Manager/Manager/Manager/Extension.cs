using KalanalyzeCode.ConfigurationManager.Ui;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

public static class Extension
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
            .AddTestUsers(TestUsers.Users);

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
    
    public static IServiceCollection AddOpenIdConnect(this IServiceCollection services)
    {
        // services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        // services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        // services.AddTransient<AuthenticationDelegatingHandler>();

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
            });
        
        return services;
    }

    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOpenApiDocument(c =>
        {
            c.Title = "Minimal APIs";
            c.Version = "v1";
        });

        return services;
    }
    
    public static WebApplication MapSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}