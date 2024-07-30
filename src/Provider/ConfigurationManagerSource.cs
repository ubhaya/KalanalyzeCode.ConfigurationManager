using KalanalyzeCode.ConfigurationManager.Provider.Options;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Provider;

public class ConfigurationManagerSource : IConfigurationSource
{
    public ConfigurationManagerSource(Action<ConfigurationOptions> optionsAction)
    {
        OptionsAction = optionsAction;
    }

    public Action<ConfigurationOptions> OptionsAction { get; }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new ConfigurationManagerProvider(this);
}