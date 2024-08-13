using LanguageExt.Common;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;

public sealed record CreateApiKeyForProjectRequest(Guid ProjectId) : IRequest<Result<Guid>>;