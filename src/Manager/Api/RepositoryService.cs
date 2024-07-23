using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;

namespace KalanalyzeCode.ConfigurationManager.Api;

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

