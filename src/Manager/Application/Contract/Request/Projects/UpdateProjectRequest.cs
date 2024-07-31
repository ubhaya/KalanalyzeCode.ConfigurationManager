using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public sealed record UpdateProjectRequest(Guid Id, string ProjectName) : IRequest;