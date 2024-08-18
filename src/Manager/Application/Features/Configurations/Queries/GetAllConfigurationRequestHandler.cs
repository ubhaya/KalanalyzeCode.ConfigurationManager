using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Configurations;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Configurations.Queries;

public sealed class GetAllConfigurationRequestHandler : 
    IRequestHandler<GetAllConfigurationRequest, GetAllConfigurationResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAllConfigurationRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllConfigurationResponse> Handle(GetAllConfigurationRequest request, 
        CancellationToken cancellationToken)
    {
        var configurations = _context.Configurations
            .Where(c=>c.ProjectId == request.ProjectId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            configurations = from configuration in configurations
                where EF.Functions.Like(configuration.Name, $"%{request.SearchString}%")
                select configuration;
        }

        configurations = request.SortLabel switch
        {
            "name_field" => configurations.OrderedByDirection(request.SortDirection, c => c.Name),
            _ => configurations
        };
        
        var totalCount = configurations.Count();

        var pagedConfigurations = await configurations.Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new GetAllConfigurationResponse(totalCount, pagedConfigurations);
    }
}