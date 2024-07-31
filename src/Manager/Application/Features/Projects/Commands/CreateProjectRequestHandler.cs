using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Commands;

public sealed class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, ResponseDataModel<CreateProjectResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDataModel<CreateProjectResponse>> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = new Project()
        {
            Name = request.ProjectName,
            Id = Guid.NewGuid()
        };

        await _context.Projects.AddAsync(project, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseModel.Create(new CreateProjectResponse()
        {
            Id = project.Id,
        });
    }
}