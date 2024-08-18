using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Projects.Queries;

public class GetAllProjectsRequestHandler : IRequestHandler<GetAllProjectsRequest, GetAllProjectsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAllProjectsRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllProjectsResponse> Handle(GetAllProjectsRequest request, CancellationToken cancellationToken)
    {
        var projects = _context.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            projects = from project in projects
                where EF.Functions.Like(project.Name, $"%{request.SearchString}%")
                select project;
        }

        projects = request.SortColumnName switch
        {
            "name_field" => projects.OrderedByDirection(request.SortDirection, p => p.Name),
            _ => projects
        };

        var totalItem = projects.Count();

        var allProjects = await projects.Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new GetAllProjectsResponse()
        {
            Projects = allProjects,
            TotalItem = totalItem
        };
    }
}