using KalanalyzeCode.ConfigurationManager.Ui.Features.WeatherForecast;

namespace KalanalyzeCode.ConfigurationManager.Ui;

public interface IWeatherForecastHandler
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default);
}