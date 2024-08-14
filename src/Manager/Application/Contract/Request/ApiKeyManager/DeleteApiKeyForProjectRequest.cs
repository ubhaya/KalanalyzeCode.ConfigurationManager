using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;

public sealed record DeleteApiKeyForProjectRequest(Guid ProjectId) : IRequest;