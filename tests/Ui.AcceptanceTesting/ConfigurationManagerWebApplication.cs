using System.Data.Common;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting;

public class ConfigurationManagerWebApplication : WebApplicationFactory<Api>, IAsyncLifetime
{
    private const string DatabaseName = "myDatabaseTest";
    private const string Username = "postgres";
    private const string DbPassword = "mysecretpassword";
    public static string HostUrl => ConfigurationHelper.GetBaseUrl();

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase(DatabaseName)
        .WithUsername(Username)
        .WithPassword(DbPassword)
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls(HostUrl);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_dbContainer.GetConnectionString())
        {
            Password = DbPassword,
            IncludeErrorDetail = true,
        };

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IDatabaseSeeder));
            services.RemoveAll(typeof(DbContextOptionsBuilder<ApplicationDbContext>));
            services.AddScoped<IDatabaseSeeder, TestSeeder>();
            services.AddScoped(s => new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionStringBuilder.ConnectionString, options =>
                    options.MigrationsHistoryTable("__AnotherMigrationsHistory"))
                .UseApplicationServiceProvider(s)
                .Options);
        });
        
        var dummy = builder.Build();
        builder.ConfigureWebHost(hostBuilder => hostBuilder.UseKestrel());
        
        var host = builder.Build();
        host.Start();
        return dummy;
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await EnsureDatabase();
        // await InitializeRespawner();
    }
    
    public async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = [
                "__EFMigrationsHistory"
            ],
        });
    }

    private async Task EnsureDatabase()
    {
        var context = Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var contextSeeder = Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await contextSeeder.SeedDataAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}