using KalanalyzeCode.ConfigurationManager.Domain.Concrete;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ConfigurationSettings> Settings { get; set; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}