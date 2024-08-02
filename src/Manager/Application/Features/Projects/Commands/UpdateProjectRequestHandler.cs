using Ardalis.GuardClauses;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Commands;

public sealed class UpdateProjectRequestHandler : IRequestHandler<UpdateProjectRequest>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Projects.FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.ProjectName;

        _context.Projects.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}