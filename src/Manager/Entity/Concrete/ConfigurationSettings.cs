using KalanalyzeCode.ConfigurationManager.Entity.Abstract;

namespace KalanalyzeCode.ConfigurationManager.Entity.Concrete;

public class ConfigurationSettings : IEntity
{
    public ConfigurationSettings(string id, string? value)
    {
        this.Id = id;
        this.Value = value;
    }

    public string Id { get;}
    public string? Value { get;}
}