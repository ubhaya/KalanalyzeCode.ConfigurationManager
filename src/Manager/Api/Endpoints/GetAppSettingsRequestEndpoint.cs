using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Api.Endpoints;

public class GetAppSettingsRequestEndpoint : IEndpointsDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet(ProjectConstant.GetAppSettings, GetAppSettings)
            .WithName($"AppSettings_{nameof(GetAppSettingsRequest)}")
            .WithTags("ApiSettings");
    }

    private async Task<ResponseDataModel<GetAppSettingsResponse>> GetAppSettings(IMediator mediator, [AsParameters] GetAppSettingsRequest request)
    {
        return await mediator.Send(request);
    }
}