using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Testcontainers.PostgreSql;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests;

public class ApiWebApplication : WebApplicationFactory<KalanalyzeCode.ConfigurationManager.Api.Api>
{
    public const string ConnectionString = "Host=localhost;Username=postgres;Database=myDatabaseTest";
    public const string DatabaseName = "myDatabaseTest";
    public const string Username = "postgres";
    public const string DbPassword = "mysecretpassword";

    public readonly PostgreSqlContainer DbContainer = new PostgreSqlBuilder()
        .WithDatabase(DatabaseName)
        .WithUsername(Username)
        .WithPassword(DbPassword)
        .Build();
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(DbContainer.GetConnectionString())
        {
            Password = DbPassword
        };
        
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp => new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionStringBuilder.ConnectionString)
                .UseApplicationServiceProvider(sp)
                .Options);
        });

        return base.CreateHost(builder);
    }
}