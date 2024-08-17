using KalanalyzeCode.ConfigurationManager.Application.Features.WeatherForecast;

namespace KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;

public interface IWeatherForecastHandler
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default);
}