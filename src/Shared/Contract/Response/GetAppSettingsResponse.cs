namespace KalanalyzeCode.ConfigurationManager.Shared.Contract.Response;

public class GetAppSettingsResponse
{
    public IEnumerable<ApplicationSettings> Settings { get; set; } = [];
}