using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Queries;

public sealed class GetProjectByIdRequestHandler : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetProjectByIdRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetProjectByIdResponse> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _context.Projects.FindAsync([request.Id], cancellationToken);
        return new GetProjectByIdResponse
        {
            Project = result
        };
    }
}