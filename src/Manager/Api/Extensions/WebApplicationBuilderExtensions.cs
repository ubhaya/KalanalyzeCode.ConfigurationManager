﻿using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace KalanalyzeCode.ConfigurationManager.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => configuration
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName));
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
    
    public static IEndpointConventionBuilder WithPermissions(this IEndpointConventionBuilder builder, Permissions permissions)
    {
        builder.RequireAuthorization(policyBuilder =>
            policyBuilder.AddRequirements(new PermissionAuthorizationRequirement(permissions)));
        return builder;
    }
}