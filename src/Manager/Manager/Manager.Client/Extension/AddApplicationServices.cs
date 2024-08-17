using KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.Interfaces;
using KalanalyzeCode.ConfigurationManager.Ui.Client.Handlers.WebAssembly;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace KalanalyzeCode.ConfigurationManager.Ui.Client.Extension;

public static class AddApplicationServices
{
    public static WebAssemblyHostBuilder AddWasmServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddHttpClient(AppConstants.HttpClient,
            client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient(AppConstants.HttpClient));

        builder.Services.Scan(scan => scan
            .FromAssemblyOf<IClient>()
            .AddClasses()
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        builder.Services.AddScoped<IWeatherForecastHandler, WeatherForecastApiHandler>();

        return builder;
    }
}