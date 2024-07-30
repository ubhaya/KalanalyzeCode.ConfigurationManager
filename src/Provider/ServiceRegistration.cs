using KalanalyzeCode.ConfigurationManager.Provider.Options;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Provider;

public static class ServiceRegistration
{
    public static IConfigurationManager AddConfigurationManager(this IConfigurationManager configuration,
        Action<ConfigurationOptions> options)
    {
        configuration.Sources.Add(new ConfigurationManagerSource
            (options));

        return configuration;
    }
}