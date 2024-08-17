using KalanalyzeCode.ConfigurationManager.Ui.Client;
using KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.Interfaces;
using KalanalyzeCode.ConfigurationManager.Ui.Contract.Request;
using KalanalyzeCode.ConfigurationManager.Ui.Mappers;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Ui.Handlers.ServerSide;

public sealed class WeatherForecastServiceHandler : IWeatherForecastHandler
{
    private readonly IMediator _mediator;

    public WeatherForecastServiceHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ICollection<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetWeatherForecastRequest(), cancellationToken);
        return result.Select(x => x.ToClientDto()).ToList();
    }
}