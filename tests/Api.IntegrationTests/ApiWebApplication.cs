using System.Data.Common;
using System.Security.Claims;
using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
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
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Util;

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

            var identityServer = WireMockServer.Start();
            
            identityServer.Given(Request.Create().WithPath("/api/Account").UsingPost())
                .RespondWith(Response.Create(() => new ResponseMessage()
                {
                    BodyData = new BodyData
                    {
                        BodyAsString = """
                                       {
                                           "apiKey": "c948a66762b8493eafd706e64efc31c6",
                                           "id": "0edc7b85-a6bc-4135-8ab0-99307ff85713",
                                           "userName": "c948a66762b8493eafd706e64efc31c6",
                                           "normalizedUserName": "C948A66762B8493EAFD706E64EFC31C6",
                                           "email": "c948a66762b8493eafd706e64efc31c6@c948a66762b8493eafd706e64efc31c6.c948a66762b8493eafd706e64efc31c6",
                                           "normalizedEmail": "C948A66762B8493EAFD706E64EFC31C6@C948A66762B8493EAFD706E64EFC31C6.C948A66762B8493EAFD706E64EFC31C6",
                                           "emailConfirmed": true,
                                           "passwordHash": "AQAAAAIAAYagAAAAEFXkDYTRC1PnkC/ZCi88N2rmu4nBs3I2UPVlAhJhqbaQUrrJe6D+PqC+7q5mhZxgMQ==",
                                           "securityStamp": "HLOCQCEA7J7S5QT74WABBSWJD5E45RZA",
                                           "concurrencyStamp": "5131b4c8-b53f-4d9e-a5cf-986f7365ecce",
                                           "phoneNumber": null,
                                           "phoneNumberConfirmed": false,
                                           "twoFactorEnabled": false,
                                           "lockoutEnd": null,
                                           "lockoutEnabled": true,
                                           "accessFailedCount": 0
                                       }
                                       """,
                    }
                }).WithStatusCode(200));
            services.AddHttpClient(AppConstants.IdentityServerClient,
                client => client.BaseAddress = new Uri(identityServer.Url?? throw new NullReferenceException()));
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