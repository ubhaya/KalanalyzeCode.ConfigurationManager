namespace KalanalyzeCode.ConfigurationManager.Api.Options;

public class OidcSettings
{
    public string? Authority { get; set; }
    public string[]? RequiredScope { get; set; }
}