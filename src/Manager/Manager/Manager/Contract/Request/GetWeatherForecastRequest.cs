using KalanalyzeCode.ConfigurationManager.Ui.Features.WeatherForecast;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Ui.Contract.Request;

public sealed record GetWeatherForecastRequest : IRequest<IEnumerable<WeatherForecast>>;