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

    public Task Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}