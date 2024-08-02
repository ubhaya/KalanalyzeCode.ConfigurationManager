using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityServer.Models.Dto;
using IdentityServer.Services.Interfaces;

namespace IdentityServer.GrantValidators;

public class ApiKeyValidator : IExtensionGrantValidator
{
    private readonly IApiKeyValidatorService _apiKeyValidator;

    public ApiKeyValidator(IApiKeyValidatorService apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public string GrantType => "api_key";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var apiKey = context.Request.Raw.Get("api_key");

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "API key missing");
            return;
        }

        var result = await IsValidApiKey(apiKey);

        if (!result.IsValid)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid API key");
            return;
        }
        
        context.Result = new GrantValidationResult(subject: result.User!.Id, authenticationMethod: GrantType);
    }

    private async Task<ApiKeyValidationResult> IsValidApiKey(string apiKey)
    {
        return await _apiKeyValidator.Validate(apiKey);
    }
}