using KalanalyzeCode.ConfigurationManager.Application.Common.Dtos;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response;

public class GetAppSettingsResponse
{
    public IEnumerable<ApplicationSettings> Settings { get; set; } = [];
}