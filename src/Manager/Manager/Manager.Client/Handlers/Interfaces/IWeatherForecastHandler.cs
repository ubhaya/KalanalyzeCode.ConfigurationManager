namespace KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.Interfaces;

public interface IWeatherForecastHandler
{
    Task<ICollection<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default);
}