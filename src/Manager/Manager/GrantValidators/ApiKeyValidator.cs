using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;

namespace KalanalyzeCode.ConfigurationManager.Ui.GrantValidators;

internal class ApiKeyValidator : IExtensionGrantValidator
{
    private readonly IApiKeyValidatorService _apiKeyValidator;

    public ApiKeyValidator(IApiKeyValidatorService apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public string GrantType => "api_key";

    public async Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var apiKeyAsString = context.Request.Raw.Get("api_key");

        if (string.IsNullOrEmpty(apiKeyAsString) || !Guid.TryParse(apiKeyAsString, out var apiKey))
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
        
        context.Result = new GrantValidationResult(subject: result.User!.Id.ToString(), authenticationMethod: GrantType);
    }

    private async Task<ApiKeyValidationResult> IsValidApiKey(Guid apiKey)
    {
        return await _apiKeyValidator.Validate(apiKey);
    }
}