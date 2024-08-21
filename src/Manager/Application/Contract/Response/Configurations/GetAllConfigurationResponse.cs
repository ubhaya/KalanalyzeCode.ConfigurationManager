using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Configurations;

public sealed record GetAllConfigurationResponse(int TotalItems, IEnumerable<Configuration> Configurations);