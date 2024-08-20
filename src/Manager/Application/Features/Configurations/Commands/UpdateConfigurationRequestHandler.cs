using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Configurations.Commands;

public sealed class UpdateConfigurationRequestHandler : IRequestHandler<UpdateConfigurationRequest>
{
    private readonly IApplicationDbContext _context;

    public UpdateConfigurationRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateConfigurationRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Configurations
            .FindAsync([request.Id], cancellationToken: cancellationToken);
        
        Guard.Against.NotFound(request.Id, entity);
        
        entity.Name = request.Name;
        entity.Value = request.Value;
        
        _context.Configurations.Update(entity);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}