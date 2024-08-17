using KalanalyzeCode.ConfigurationManager.Ui.Contract.Request;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Ui.Features.WeatherForecast;

public sealed class GetWeatherForecastRequestHandler : IRequestHandler<GetWeatherForecastRequest, IEnumerable<WeatherForecast>>
{
    private readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly Random _random = new(420);
    
    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(_random.Next(100, 2000), cancellationToken);
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                100,
                _summaries[Random.Shared.Next(_summaries.Length)]
            ));
        return forecast;
    }
}

public sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}