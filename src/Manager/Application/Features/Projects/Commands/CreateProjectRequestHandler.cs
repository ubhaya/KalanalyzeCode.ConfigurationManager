using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Commands;

public sealed class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = new Project()
        {
            Name = request.ProjectName,
            Id = Guid.NewGuid()
        };

        await _context.Projects.AddAsync(project, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProjectResponse()
        {
            Project = project,
        };
    }
}