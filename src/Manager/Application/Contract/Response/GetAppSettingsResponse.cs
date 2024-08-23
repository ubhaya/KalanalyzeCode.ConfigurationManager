using KalanalyzeCode.ConfigurationManager.Application.Common.Dtos;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response;

public class GetAppSettingsResponse
{
    public ICollection<Configuration> Configurations { get; set; } = [];
}