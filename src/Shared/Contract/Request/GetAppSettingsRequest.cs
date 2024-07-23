namespace KalanalyzeCode.ConfigurationManager.Shared.Contract.Request;

public class GetAppSettingsRequest : IHttpRequest
{
    public string SettingName { get; set; } = string.Empty;
}