using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.Projects;

public sealed record DeleteProjectRequest(Guid Id) : IRequest;