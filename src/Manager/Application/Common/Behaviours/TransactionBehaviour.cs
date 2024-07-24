using KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
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
            logger.LogError(AppConstants.LoggingMessages.TransactionBehaviour);

            await context.RollbackTransaction(cancellationToken);
            throw;
        }
    }
}