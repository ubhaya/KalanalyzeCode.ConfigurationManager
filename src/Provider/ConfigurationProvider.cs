using System.Net.Http.Json;
using KalanalyzeCode.ConfigurationManager.Shared;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Provider;

public class ConfigurationManagerSource : IConfigurationSource
{
    public ConfigurationManagerSource(Action<ConfigurationOptions> optionsAction, bool? reloadPeriodically = null,
        int? periodInSeconds = null)
    {
        OptionsAction = optionsAction;
        ReloadPeriodically = reloadPeriodically ?? false;
        PeriodInSeconds = periodInSeconds ?? 5;
    }

    public Action<ConfigurationOptions> OptionsAction { get; }

    public bool ReloadPeriodically { get; }

    public int PeriodInSeconds { get; }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new ConfigurationManagerProvider(this);
}

public class ConfigurationOptions
{
    public Uri? BaseAddress { get; private set; }

    public void SetBaseAddress(string baseAddress)
    {
        BaseAddress = new Uri(baseAddress);
    }

    public void SetBaseAddress(Uri baseAddress)
    {
        BaseAddress = baseAddress;
    }
}

public class ConfigurationManagerProvider : ConfigurationProvider, IDisposable
{
    private readonly Timer? _timer;

    public ConfigurationManagerProvider(ConfigurationManagerSource source)
    {
        Source = source;

        if (Source.ReloadPeriodically)
        {
            _timer = new Timer(
                callback: ReloadSettings,
                dueTime: TimeSpan.FromSeconds(10),
                period: TimeSpan.FromSeconds(Source.PeriodInSeconds),
                state: null
            );
        }
    }

    private ConfigurationManagerSource Source { get; }

    public override void Load()
    {
        var options = new ConfigurationOptions();

        Source.OptionsAction(options);

        var client = new HttpClient()
        {
            BaseAddress = options.BaseAddress
        };

        var result = Task.Run(async () => await client.GetFromJsonAsync<IEnumerable<ApplicationSettings>>(
                "/appsettings?settingName=StarfishOptions"))
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

public static class ServiceRegistration
{
    public static IConfigurationManager AddConfigurationManager(this IConfigurationManager configuration,
        Action<ConfigurationOptions> options)
    {
        configuration.Sources.Add(new ConfigurationManagerSource
            (options, true, 5));

        return configuration;
    }
}