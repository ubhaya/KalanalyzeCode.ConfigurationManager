using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;
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

    public static WebApplication MediateGet<TRequest>(
        this WebApplication app,
        string template, string name, string groupName, bool isAuthorized, params string[] tags) where TRequest : IHttpRequest
    {
        app.MapGet(template,
            async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request))
            .WithName($"{groupName}_{name}")
            .WithGroupName(groupName)
            .WithTags(tags)
            .RequireAuthorization();
        
        return app;
    }

    public static async Task<WebApplication> SeedDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var dbSeeder = services.GetRequiredService<IDatabaseSeeder>();
                await dbSeeder.InitializeAsync();
                await dbSeeder.SeedAsync();
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
}