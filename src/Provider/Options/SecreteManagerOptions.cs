namespace KalanalyzeCode.ConfigurationManager.Provider.Options;

public class SecreteManagerOptions
{
    public Uri? BaseAddress { get; set; }
    public string ApiKey { get; set; } = string.Empty;
}