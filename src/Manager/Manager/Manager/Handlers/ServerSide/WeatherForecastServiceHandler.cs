using KalanalyzeCode.ConfigurationManager.Ui.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Ui.Features.WeatherForecast;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Ui.Handlers.ServerSide;

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