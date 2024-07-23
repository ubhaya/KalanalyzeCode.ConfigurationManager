using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Shared;

namespace KalanalyzeCode.ConfigurationManager.Application;

public class RepositoryService
{
    private readonly List<ConfigurationSettings> _configurationSettingsList =
    [
        new ConfigurationSettings("StarfishOptions:FraudCheckerEnabled", "true"),
        new ConfigurationSettings("StarfishOptions:PerformanceMonitorEnabled", "false")
    ];
    
    public Task<List<ApplicationSettings>> GetAllApplicationSettings(string settingName)
    {
        return Task.FromResult(_configurationSettingsList.Where(l => l.Id.StartsWith(settingName))
            .Select(s => new ApplicationSettings(s.Id, s.Value)).ToList());
    }
}

