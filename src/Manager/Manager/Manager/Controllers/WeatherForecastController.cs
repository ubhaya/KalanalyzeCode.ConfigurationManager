using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Features.WeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

[ApiController]
[PermissionAuthorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Permissions = Permissions.All)]
[Route("api/[controller]")]
public sealed class WeatherForecastController : ApiControllerBase
{
    public WeatherForecastController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetWeatherForecastRequest(), cancellationToken);
    }
}