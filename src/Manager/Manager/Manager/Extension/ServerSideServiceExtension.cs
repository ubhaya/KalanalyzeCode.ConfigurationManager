using KalanalyzeCode.ConfigurationManager.Ui.Services.Implementations;
using KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;

namespace KalanalyzeCode.ConfigurationManager.Ui.Extension;

public static class ServerSideServiceExtension
{
    public static IServiceCollection AddServerSideServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        
        services.AddScoped<IWeatherForecastHandler, WeatherForecastServiceHandler>();

        return services;
    }
}