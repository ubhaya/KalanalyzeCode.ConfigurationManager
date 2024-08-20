using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Configurations.Commands;

public sealed class DeleteConfigurationRequestHandler : IRequestHandler<DeleteConfigurationRequest>
{
    private readonly IApplicationDbContext _context;

    public DeleteConfigurationRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteConfigurationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Configurations.FindAsync([request.Id], cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);

        _context.Configurations.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}