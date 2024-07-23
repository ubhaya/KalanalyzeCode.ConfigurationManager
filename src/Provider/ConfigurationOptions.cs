namespace KalanalyzeCode.ConfigurationManager.Provider;

public class ConfigurationOptions
{
    public Uri? BaseAddress { get; set; }
    public bool ReloadPeriodically { get; set; } = false;
    public int PeriodInSeconds { get; set; } = 5;
}