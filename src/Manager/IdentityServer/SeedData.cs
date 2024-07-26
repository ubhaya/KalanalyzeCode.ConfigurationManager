using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

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
