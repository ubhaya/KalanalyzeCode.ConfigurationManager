using System.Net.Http.Json;
using KalanalyzeCode.ConfigurationManager.Shared;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Provider;

public class ConfigurationManagerProvider : ConfigurationProvider, IDisposable
{
    private readonly Timer? _timer;

    public ConfigurationManagerProvider(ConfigurationManagerSource source)
    {
        Source = source;

        source.OptionsAction(Options);

        if (Options.ReloadPeriodically)
        {
            _timer = new Timer(
                callback: ReloadSettings,
                dueTime: TimeSpan.FromSeconds(10),
                period: TimeSpan.FromSeconds(Options.PeriodInSeconds),
                state: null
            );
        }
    }

    private ConfigurationManagerSource Source { get; }
    private ConfigurationOptions Options { get; } = new();

    public override void Load()
    {
        var options = new ConfigurationOptions();

        Source.OptionsAction(options);

        var client = new HttpClient()
        {
            BaseAddress = options.BaseAddress
        };

        var result = Task.Run(async () => await client.GetFromJsonAsync<IEnumerable<ApplicationSettings>>(
                "/api/appsettings?settingName=StarfishOptions"))
            .Result;

        Data = result?
                   .ToDictionary<ApplicationSettings, string, string?>(c => c.Id, c => c.Value,
                       StringComparer.OrdinalIgnoreCase) ??
               [];
    }

    private void ReloadSettings(object? state)
    {
        Load();
        OnReload();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}