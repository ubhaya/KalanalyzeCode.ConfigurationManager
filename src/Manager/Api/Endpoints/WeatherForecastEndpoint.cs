using Identity.Shared.Authorization;
using KalanalyzeCode.ConfigurationManager.Api.Extensions;

namespace KalanalyzeCode.ConfigurationManager.Api.Endpoints;

public class WeatherForecastEndpoint : IEndpointsDefinition
{
    private readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", GetWeatherForecast)
            .WithPermissions(Permissions.All)
            .WithName("WeatherForecast_GetWeatherForecast")
            .WithTags("WeatherForecast");
    }
    
    private WeatherForecast[] GetWeatherForecast(HttpContext context)
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    100,
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray();
        return forecast;
    }
}