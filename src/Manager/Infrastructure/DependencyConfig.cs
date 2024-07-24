using FluentValidation;
using KalanalyzeCode.ConfigurationManager.Application;
using KalanalyzeCode.ConfigurationManager.Application.Common.Behaviours;
using KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Identity;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure;

public static class DependencyConfig
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityServer();
        services.AddPersistence(config);
        return services;
    }
    
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients())
            .AddTestUsers(Config.Users);

        services.AddAuthentication();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config["PostgreSql:ConnectionString"];
        var dbPassword = config["PostgreSql:DbPassword"];

        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Password = dbPassword
        };

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.ConnectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDatabaseSeeder, ApplicationDbContextSeeder>();
        
        return services;
    }
}