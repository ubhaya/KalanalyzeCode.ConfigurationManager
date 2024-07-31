using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Api.Controllers;


public class AppSettingsController : ApiControllerBase
{
    public AppSettingsController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Permissions.GetAppSettings)]
    public async Task<ActionResult<ResponseDataModel<GetAppSettingsResponse>>> Get(
        [FromQuery] GetAppSettingsRequest request, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}