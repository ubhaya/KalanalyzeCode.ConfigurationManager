namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Options;

public class OidcSettings
{
    public string? Authority { get; set; }
    public string[]? RequiredScope { get; set; }
}