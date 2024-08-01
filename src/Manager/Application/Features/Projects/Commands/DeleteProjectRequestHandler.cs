using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Commands;

public sealed class DeleteProjectRequestHandler : IRequestHandler<DeleteProjectRequest>
{
    private readonly IApplicationDbContext _context;

    public DeleteProjectRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Projects.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}