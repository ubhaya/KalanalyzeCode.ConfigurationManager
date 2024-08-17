using KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.Interfaces;

namespace KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.WebAssembly;

public sealed class WeatherForecastApiHandler : IWeatherForecastHandler
{
    private readonly IWeatherForecastClient _client;

    public WeatherForecastApiHandler(IWeatherForecastClient client)
    {
        _client = client;
    }

    public async Task<ICollection<WeatherForecast>> GetWeatherForecast(CancellationToken cancellationToken = default)
    {
        return await _client.GetAsync(cancellationToken);
    }
}