using LanguageExt.Common;
using MediatR;

namespace KalanalyzeCode.ConfigurationManager.Application.Contract.Response.ApiKeyManager;

public sealed record CreateApiKeyForProjectResponse(Guid ApiKey) : IRequest<Result<Guid>>;