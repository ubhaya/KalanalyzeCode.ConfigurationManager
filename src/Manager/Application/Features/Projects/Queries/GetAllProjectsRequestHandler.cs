using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
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
        var projects = _context.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            projects = from project in projects
                where project.Name.Contains(request.SearchString, StringComparison.OrdinalIgnoreCase)
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
        
        var response = new GetAllProjectsResponse()
        {
            Projects = allProjects,
            TotalItem = totalItem
        };
        return ResponseModel.Create(response);
    }
}