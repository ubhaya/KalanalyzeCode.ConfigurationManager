using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests;

public class TestBase : IAsyncLifetime
{
    protected readonly ApiWebApplication Application;

    public TestBase()
    {
        Application = new ApiWebApplication();
    }

    public async Task InitializeAsync()
    {
        await Application.DbContainer.StartAsync();
        using (var scope = Application.Services.CreateScope())
        {
            await EnsureDatabase(scope);
        }
    }

    protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    protected async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }
    
    private async Task EnsureDatabase(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await Application.DbContainer.StopAsync();
        await Application.DisposeAsync();
    }
}