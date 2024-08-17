using NetEscapades.EnumGenerators;

namespace KalanalyzeCode.ConfigurationManager.Application.Authorization;

[Flags]
[EnumExtensions]
public enum Permissions
{
    None = 0,
    AppSettingsRead = 1,
    AppSettingsWrite = 2,
    ProjectRead = 4,
    ProjectWrite = 8,
    WeatherForecast = 16,
    All = ~None,
}