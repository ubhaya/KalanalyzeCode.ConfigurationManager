using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, GetAppSettingsResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAppSettingsRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetAppSettingsResponse> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p=>p.Configurations)
            .Where(p => p.Name == request.ProjectName)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (project == null)
            return new GetAppSettingsResponse();

        return new GetAppSettingsResponse
        {
            Configurations = project.Configurations
        };
    }
}