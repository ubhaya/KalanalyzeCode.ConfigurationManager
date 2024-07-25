using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using MediatR;
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

    public static RouteHandlerBuilder MediateGet<TRequest>(
        this WebApplication app,
        string template) where TRequest : IHttpRequest
    {
        return app.MapGet(template,
            async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
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
    
    public static RouteHandlerBuilder RequireAuthorization(this RouteHandlerBuilder builder, Permissions permissions)
    {
        builder.RequireAuthorization(policyBuilder =>
            policyBuilder.AddRequirements(new PermissionAuthorizationRequirement(permissions)));
        return builder;
    }
}