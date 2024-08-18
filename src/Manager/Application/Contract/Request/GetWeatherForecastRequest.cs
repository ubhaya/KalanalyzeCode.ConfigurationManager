using KalanalyzeCode.ConfigurationManager.Application.Features.WeatherForecast;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request;

public sealed record GetWeatherForecastRequest : IRequest<IEnumerable<WeatherForecast>>;