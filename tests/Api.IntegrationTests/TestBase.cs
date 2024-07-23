using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace Api.IntegrationTests;

public class TestBase : IAsyncLifetime
{
    protected readonly ApiWebApplication Application;

    public TestBase()
    {
        Application = new ApiWebApplication();

        using var scope = Application.Services.CreateScope();
        EnsureDatabase(scope);
    }

    public async Task InitializeAsync()
    {
        await ResetState();
    }

    protected Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();
        // var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        // context.Add(entity);
        // await context.SaveChangesAsync();
        return Task.FromResult(entity);
    }

    protected Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        throw new NotImplementedException();
        // using var scope = Application.Services.CreateScope();
        // var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        // return await context.FindAsync<TEntity>(keyValues);
    }
    
    private void EnsureDatabase(IServiceScope scope)
    {
        // var context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        // context.Database.Migrate();
    }

    private static Task ResetState()
    {
        return Task.CompletedTask;
        // var checkpoint = await Respawner.CreateAsync(ApiWebApplication.TestConnectionString, new RespawnerOptions
        // {
        //     TablesToIgnore = new Table[]
        //     {
        //         "__EFMigrationsHistory"
        //     }
        // });
        // await checkpoint.ResetAsync(ApiWebApplication.TestConnectionString);
    }

    public Task DisposeAsync()
    {
        Application.Dispose();
        return Task.CompletedTask;
    }
}