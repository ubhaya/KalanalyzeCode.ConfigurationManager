using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse>(
    IApplicationDbContext context,
    ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await context.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await context.CommitTransactionAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            logger.LogError("Request failed: Rolling back all the changes made to the Context");

            await context.RollbackTransaction(cancellationToken);
            throw;
        }
    }
}