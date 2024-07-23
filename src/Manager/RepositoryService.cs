using KalanalyzeCode.ConfigurationManager.Shared;

namespace KalanalyzeCode.ConfigurationManager;

public class RepositoryService
{
    private readonly List<ConfigurationSettings> _configurationSettingsList =
    [
        new ConfigurationSettings("StarfishOptions:FraudCheckerEnabled", "true"),
        new ConfigurationSettings("StarfishOptions:PerformanceMonitorEnabled", "false")
    ];
    
    public Task<IEnumerable<ApplicationSettings>> GetAllApplicationSettings(string settingName)
    {
        return Task.FromResult(_configurationSettingsList.Where(l => l.Id.StartsWith(settingName))
            .Select(s => new ApplicationSettings(s.Id, s.Value)));
    }
}

internal record ConfigurationSettings(string Id, string? Value);