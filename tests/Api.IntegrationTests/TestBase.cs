using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests;

public class TestBase
{ 
    protected async Task<TEntity> AddAsync<TEntity>(IServiceScope scope, TEntity entity) where TEntity : class
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    protected async Task<TEntity?> FindAsync<TEntity>(IServiceScope scope, params object[] keyValues) where TEntity : class
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }
}