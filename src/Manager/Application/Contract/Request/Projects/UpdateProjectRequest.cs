using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public sealed class UpdateProjectRequest : IRequest
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
}