using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class SeedDataExtension
{
    public static async Task<WebApplication> SeedData(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var contextSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await contextSeeder.SeedDataAsync();

        return app;
    } 
}