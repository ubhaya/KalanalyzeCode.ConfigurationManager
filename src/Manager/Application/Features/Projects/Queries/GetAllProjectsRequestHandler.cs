using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Queries;

public class GetAllProjectsRequestHandler : IRequestHandler<GetAllProjectsRequest, ResponseDataModel<GetAllProjectsResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAllProjectsRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDataModel<GetAllProjectsResponse>> Handle(GetAllProjectsRequest request, CancellationToken cancellationToken)
    {
        var allProjects = await _context.Projects.ToListAsync(cancellationToken);
        var response = new GetAllProjectsResponse()
        {
            Projects = allProjects
        };
        return ResponseModel.Create(response);
    }
}