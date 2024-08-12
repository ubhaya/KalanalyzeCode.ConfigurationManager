using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ProjectManager;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using LanguageExt;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.ProjectsManager.Queries;

public sealed class GetProjectInformationRequestHandler : IRequestHandler<GetProjectInformationRequest, Option<Project>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectInformationRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Option<Project>> Handle(GetProjectInformationRequest request, CancellationToken cancellationToken)
    {
        return await _context.Projects
            .FindAsync([request.Id], cancellationToken: cancellationToken);
    }
}