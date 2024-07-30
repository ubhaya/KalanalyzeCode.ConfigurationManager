using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.Endpoints;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace KalanalyzeCode.ConfigurationManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }

    public static async Task<WebApplication> SeedDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                if (context.Database.IsNpgsql())
                {
                    await context.Database.MigrateAsync();
                }

                var dbSeeder = services.GetRequiredService<IDatabaseSeeder>();
                await dbSeeder.SeedSampleDataAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, AppConstants.LoggingMessages.SeedingOrMigrationError);
                throw;
            }
        }
        return app;
    }
    
    public static IEndpointConventionBuilder RequireAuthorization(this IEndpointConventionBuilder builder, Permissions permissions)
    {
        builder.RequireAuthorization(policyBuilder =>
            policyBuilder.AddRequirements(new PermissionAuthorizationRequirement(permissions)));
        return builder;
    }
    
    public static void AddEndpointDefinitions(this IServiceCollection services, params Type[] scanMakers)
    {
        var endpointDefinitions = new List<IEndpointsDefinition>();

        foreach (var maker in scanMakers)
        {
            endpointDefinitions.AddRange(
                maker.Assembly.ExportedTypes
                    .Where(x=>typeof(IEndpointsDefinition).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance).Cast<IEndpointsDefinition>()
            );
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointsDefinition>);
    }

    public static void UseEndpointDefinition(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointsDefinition>>();

        foreach (var endpointsDefinition in definitions)
        {
            endpointsDefinition.DefineEndpoints(app);
        }
    }
}