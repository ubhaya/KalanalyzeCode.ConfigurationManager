using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KalanalyzeCode.ConfigurationManager.Entity.Entities;

public class Configuration
{
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Project))]
    public Guid ProjectId { get; set; }
    
    [JsonIgnore]
    public Project? Project { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
}