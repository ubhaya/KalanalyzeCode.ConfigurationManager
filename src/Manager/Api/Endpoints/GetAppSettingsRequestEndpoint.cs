using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.Extensions;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.Endpoints;

public class GetAppSettingsRequestEndpoint : IEndpointsDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(AppConstants.Routes.GetAppSettings, GetAppSettings)
            .WithPermissions(Permissions.GetAppSettings)
            .WithName($"AppSettings_{nameof(GetAppSettingsRequest)}")
            .WithTags("ApiSettings");
    }

    private async Task<ResponseDataModel<GetAppSettingsResponse>> GetAppSettings(IMediator mediator, [AsParameters] GetAppSettingsRequest request)
    {
        return await mediator.Send(request);
    }
}