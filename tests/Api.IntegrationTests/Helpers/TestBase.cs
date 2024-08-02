using AutoFixture;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class TestBase : IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;
    protected readonly IApplicationDbContext Context;
    private CancellationTokenSource? _cancellationTokenSource;
    protected CancellationToken CancellationToken;
    protected IFixture Fixture = new Fixture();
    
    public TestBase(ApiWebApplication factory)
    {
        _scope = factory.Scope;
        _resetDatabase = factory.ResetDatabaseAsync;
        Context = factory.DatabaseContext;
    }
    
    protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Add(entity);
        await context.SaveChangesAsync(CancellationToken);
        return entity;
    }
    
    protected async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(ICollection<TEntity> entity) where TEntity : class
    {
        var context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.AddRangeAsync(entity, CancellationToken);
        await context.SaveChangesAsync(CancellationToken);
        return entity;
    }

    protected async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        var context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }

    public Task InitializeAsync()
    {
        CancellationToken = (_cancellationTokenSource ?? new CancellationTokenSource()).Token;
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
            return _resetDatabase();
        }

        return _resetDatabase();
    }
}