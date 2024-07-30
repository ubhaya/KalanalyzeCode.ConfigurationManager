namespace KalanalyzeCode.ConfigurationManager.Provider.Options;

public class SecreteManagerOptions
{
    public Uri? BaseAddress { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public string ClientSecrete { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    internal string Scope => string.Join(" ", Scopes);
    public IEnumerable<string> Scopes { private get; set; } = [];
}