using System.Text.Json.Serialization;

namespace KalanalyzeCode.ConfigurationManager.Entity.Entities;

public sealed class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ApiKey { get; set; }

    [JsonIgnore]
    public ICollection<Configuration> Configurations { get; set; } = [];
}