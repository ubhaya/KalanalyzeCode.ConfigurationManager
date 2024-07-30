using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public class SeedData
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();

                var contextSeeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
                await contextSeeder.SeedDataAsync();
            }
        }
    }
}
