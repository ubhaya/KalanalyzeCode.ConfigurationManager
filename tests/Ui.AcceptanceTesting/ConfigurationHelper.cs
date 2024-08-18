using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace KalanalyzeCode.ConfigurationManager.Ui.AcceptanceTesting;

public static class ConfigurationHelper
{
    private static readonly IConfiguration Configuration;

    static ConfigurationHelper()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#if DEBUG
            .AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true)
#endif
            .AddEnvironmentVariables()
            .Build();
    }

    private static string? _baseUrl;
    private static string? _dockerComposeFileName;

    public static string GetDockerComposeFileName()
    {
        if (string.IsNullOrWhiteSpace(_dockerComposeFileName))
        {
            _dockerComposeFileName = Configuration["DockerComposeFileName"];
            Guard.Against.NullOrWhiteSpace(_dockerComposeFileName);
        }

        return _dockerComposeFileName;
    }

    public static string GetBaseUrl()
    {
        if (string.IsNullOrWhiteSpace(_baseUrl))
        {
            _baseUrl = Configuration["BaseUrl"];
            _baseUrl = _baseUrl!.TrimEnd('/');
        }

        return _baseUrl;
    }
}