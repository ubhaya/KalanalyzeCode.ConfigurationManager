using IdentityServer.Data;
using KalanalyzeCode.ConfigurationManager.Ui.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class DatabaseServiceExtension
{
    public static WebApplicationBuilder AddDatabaseConnect(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["PostgreSql:ConnectionString"];
        var dbPassword = builder.Configuration["PostgreSql:DbPassword"];

        var postgrSqlBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Password = dbPassword
        };

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(postgrSqlBuilder.ConnectionString));

        builder.Services.AddScoped<ApplicationDbContextSeeder>();

        return builder;
    }

    public static async Task<WebApplication> SeedData(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        var contextSeeder = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeder>();
        await contextSeeder.SeedDataAsync();

        return app;
    }
}