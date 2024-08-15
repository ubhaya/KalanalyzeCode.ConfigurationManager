using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.Options;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace KalanalyzeCode.ConfigurationManager.Api;

public static class DependencyConfig
{
    public static IServiceCollection AddWebApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy(name: AppConstants.CorsPolicy,
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOpenApiDocument(c =>
        {
            c.Title = "Minimal APIs";
            c.Version = "v1";
        });

        var oidcSettings = new OidcSettings();
        configuration.GetRequiredSection(nameof(OidcSettings)).Bind(oidcSettings);
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = oidcSettings.Authority;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("api_scope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", oidcSettings.RequiredScope??Enumerable.Empty<string>());
            });
        
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddHttpClient(AppConstants.IdentityServerClient,
            client => client.BaseAddress = new Uri("https://localhost:5001"));

        return services;
    }

    public static WebApplication MapSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}