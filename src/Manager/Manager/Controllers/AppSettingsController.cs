using System.Security.Claims;
using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Response;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;


public class AppSettingsController : ApiControllerBase
{
    public AppSettingsController(ISender mediator) : base(mediator)
    {
    }

    [HttpGet]
    [PermissionAuthorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Permissions = Permissions.AppSettingsRead)]
    public async Task<ActionResult<GetAppSettingsResponse>> Get(
        [FromQuery] GetAppSettingsRequest request, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}