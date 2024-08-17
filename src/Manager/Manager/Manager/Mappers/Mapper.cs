using Riok.Mapperly.Abstractions;

namespace KalanalyzeCode.ConfigurationManager.Ui.Mappers;

[Mapper]
public static partial class Mapper
{
    public static partial Client.WeatherForecast ToClientDto(
        this Features.WeatherForecast.WeatherForecast weatherForecast);
}