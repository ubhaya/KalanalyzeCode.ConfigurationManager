using IdentityServer.Models.Dto;

namespace IdentityServer.Services.Interfaces;

public interface IApiKeyValidatorService
{
    Task<ApiKeyValidationResult> Validate(string apiKey, CancellationToken cancellationToken = default);
}