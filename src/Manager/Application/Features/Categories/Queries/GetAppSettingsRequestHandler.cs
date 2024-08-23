using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.Categories.Queries;

public class GetAppSettingsRequestHandler : IRequestHandler<GetAppSettingsRequest, GetAppSettingsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAppSettingsRequestHandler(IApplicationDbContext context, IAuthorizationService authorizationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetAppSettingsResponse> Handle(GetAppSettingsRequest request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Configurations)
            .Where(p => p.Name == request.ProjectName)
            .SingleOrDefaultAsync(cancellationToken);

        if (project == null)
            return new GetAppSettingsResponse();

        var user = _httpContextAccessor.HttpContext?.User!;

        var authorizationResult = await _authorizationService.AuthorizeAsync(user, project, "SettingsPolicy");

        if (authorizationResult.Succeeded)
        {
            return new GetAppSettingsResponse
            {
                Configurations = project.Configurations
            };
        }
        throw new UnauthorizedAccessException();
    }
}