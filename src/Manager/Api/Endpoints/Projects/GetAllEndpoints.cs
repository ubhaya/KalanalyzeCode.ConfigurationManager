using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.Extensions;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Projects;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.Endpoints.Projects;

public class GetAllEndpoints : IEndpointsDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(AppConstants.Routes.Projects.GetAll, GetAllProjects)
            .WithPermissions(Permissions.ProjectRead)
            .WithName($"Projects_GetAll")
            .WithTags("Projects");
    }

    private async Task<ResponseDataModel<GetAllProjectsResponse>> GetAllProjects(IMediator mediator,
        [AsParameters] GetAllProjectsRequest request, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(request, cancellationToken);
    }
}