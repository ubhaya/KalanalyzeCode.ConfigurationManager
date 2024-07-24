using System.Reflection;
using Duende.IdentityServer;
using KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Identity;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Options;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;
using KalanalyzeCode.ConfigurationManager.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure;

public static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistence(config);
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["PostgreSql:ConnectionString"];
        var dbPassword = configuration["PostgreSql:DbPassword"];

        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Password = dbPassword
        };

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.ConnectionString);
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, FlexibleAuthorizationPolicyProvider>();

        var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;
        
        var identityBuilder = services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(builder.ConnectionString, sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b =>
                    b.UseNpgsql(builder.ConnectionString, sql =>
                    {
                        sql.MigrationsAssembly(migrationsAssembly);
                    });
            })
            .AddAspNetIdentity<ApplicationUser>();

        identityBuilder.AddProfileService<ProfileService>();
        
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
            })
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("api_scope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", oidcSettings.RequiredScope??Enumerable.Empty<string>());
            });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDatabaseSeeder, ApplicationDbContextSeeder>();
        
        return services;
    }
}