using System.Data.Common;
using System.Security.Claims;
using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests;

public class ApiWebApplication : WebApplicationFactory<Api>, IAsyncLifetime
{
    private const string DatabaseName = "myDatabaseTest";
    private const string Username = "postgres";
    private const string DbPassword = "mysecretpassword";

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase(DatabaseName)
        .WithUsername(Username)
        .WithPassword(DbPassword)
        .Build();

    private readonly MockAuthUser _user = new([
        new Claim("sub", Guid.NewGuid().ToString()),
        new Claim("email", "default-user@xyz.com"),
        new Claim("scope", "KalanalyzeCode.ConfigurationManager"),
        new Claim("scope", "profile"),
        new Claim("scope", "openid"),
        new Claim(CustomClaimTypes.Permissions, ((int)Permissions.All).ToString())]);

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    
    public HttpClient HttpClient { get; private set; } = default!;
    public IServiceScope Scope { get; private set; } = default!;
    public IApplicationDbContext DatabaseContext { get; set; } = default!;
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_dbContainer.GetConnectionString())
        {
            Password = DbPassword
        };
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IDatabaseSeeder));
            services.AddScoped<IDatabaseSeeder, TestSeeder>();
            services.AddTestAuthentication();
            services.AddScoped(_ => _user);
            services.AddScoped(sp => new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionStringBuilder.ConnectionString)
                .UseApplicationServiceProvider(sp)
                .Options);
        });

        return base.CreateHost(builder);
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Scope = Services.CreateScope();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        DatabaseContext = Scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        await EnsureDatabase();
        HttpClient = CreateClient();
        await InitializeRespawner();
    }
    
    private async Task InitializeRespawner()
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
        var context = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}