using KalanalyzeCode.ConfigurationManager.Application.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Application.Features.WeatherForecast;
using KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Ui.Services.Implementations;

public sealed class WeatherForecastServiceHandler : IWeatherForecastHandler
{
    private readonly IMediator _mediator;

    public WeatherForecastServiceHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default)
    {
       return await _mediator.Send(new GetWeatherForecastRequest(), cancellationToken);
    }
}