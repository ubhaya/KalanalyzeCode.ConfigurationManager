namespace KalanalyzeCode.ConfigurationManager.Shared;

public class ProjectConstant
{
    private const string Api = nameof(Api);
    public const string GetAppSettings = $"{nameof(Api)}/{nameof(GetAppSettings)}";

    public const string ConfigurationManagerApi = "configuration-manager-api";
    public const string ConfigurationManagerUi = "configuration-manager-ui";
}