using KalanalyzeCode.ConfigurationManager.Ui.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Ui.Features.WeatherForecast;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KalanalyzeCode.ConfigurationManager.Ui.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme}, {CookieAuthenticationDefaults.AuthenticationScheme}")]
[Route("api/[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetWeatherForecastRequest(), cancellationToken);
    }
}