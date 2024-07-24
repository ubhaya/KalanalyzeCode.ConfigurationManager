﻿using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Abstract;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;

public interface IApplicationDbContext
{
    DbSet<ConfigurationSettings> Settings { get; set; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IPublisher _publisher;
    private readonly ILogger<ApplicationDbContext> _logger;
    private IDbContextTransaction? _currentTransaction;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher,
        ILogger<ApplicationDbContext> logger) : base(options)
    {
        _publisher = publisher;
        _logger = logger;
    }
    
    public DbSet<ConfigurationSettings> Settings { get; set; }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            _logger.LogInformation(AppConstants.LoggingMessages.TransactionAlreadyCreated, _currentTransaction.TransactionId);
            return;
        }
    
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
        _logger.LogInformation(AppConstants.LoggingMessages.TransactionCreated, _currentTransaction.TransactionId);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            return;
        }
        
        _logger.LogInformation(AppConstants.LoggingMessages.TransactionCommited, _currentTransaction.TransactionId);
    
        await _currentTransaction.CommitAsync(cancellationToken);
    
        _currentTransaction.Dispose();
        _currentTransaction = null;
    }
    
    public async Task RollbackTransaction(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            return;
        }
    
        _logger.LogDebug(AppConstants.LoggingMessages.TransactionRollingBack, _currentTransaction.TransactionId);
    
        await _currentTransaction.RollbackAsync(cancellationToken);
    
        _currentTransaction.Dispose();
        _currentTransaction = null;
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
    
        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
    
        foreach (var @event in events)
        {
            @event.IsPublished = true;
    
            _logger.LogInformation(AppConstants.LoggingMessages.NewDomainEvent, @event.GetType().Name);
    
            // Note: If an unhandled exception occurs, all the saved changes will be rolled back
            // by the TransactionBehavior. All the operations related to a domain event finish
            // successfully or none of them do.
            // Reference: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation#what-is-a-domain-event
            await _publisher.Publish(@event, cancellationToken);
        }
    
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}