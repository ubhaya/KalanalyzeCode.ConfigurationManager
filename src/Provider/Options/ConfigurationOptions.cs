namespace KalanalyzeCode.ConfigurationManager.Provider.Options;

public class ConfigurationOptions
{
    public SecreteManagerOptions SecreteManagerOptions { get; set; } = new();
    public bool ReloadPeriodically { get; set; } = false;
    public int PeriodInSeconds { get; set; } = 5;
}