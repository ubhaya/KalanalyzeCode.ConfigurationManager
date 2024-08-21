using KalanalyzeCode.ConfigurationManager.Application.Contract.Response.Configurations;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Configurations;

public sealed class CreateConfigurationRequest : IRequest<CreateConfigurationResponse>
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}