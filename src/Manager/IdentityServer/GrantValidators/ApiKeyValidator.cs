using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace IdentityServer.GrantValidators;

public class ApiKeyValidator : IExtensionGrantValidator
{
    public string GrantType => "api_key";

    public Task ValidateAsync(ExtensionGrantValidationContext context)
    {
        var apiKey = context.Request.Raw.Get("api_key");

        if (string.IsNullOrEmpty(apiKey))
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "API key missing");
            return Task.CompletedTask;
        }

        // Validate the API key (this is where you implement your logic to check the API key)
        if (IsValidApiKey(apiKey))
        {
            // Create a subject identifier (this could be a user ID or something similar)
            context.Result = new GrantValidationResult(subject: "user_id", authenticationMethod: GrantType);
        }
        else
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid API key");
        }
        
        return Task.CompletedTask;
    }

    private bool IsValidApiKey(string apiKey)
    {
        // Todo: Implement your API key validation logic here
        return true;
    }
}