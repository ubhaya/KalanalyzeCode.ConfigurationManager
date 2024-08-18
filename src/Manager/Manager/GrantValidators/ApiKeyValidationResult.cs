using KalanalyzeCode.ConfigurationManager.Application.Common.Models;

namespace KalanalyzeCode.ConfigurationManager.Ui.GrantValidators;

internal class ApiKeyValidationResult
{

    private ApiKeyValidationResult(bool isValid, IEnumerable<string> messages)
    {
        IsValid = isValid;
        Messages = messages;
    }

    private ApiKeyValidationResult(ApplicationUser user)
    {
        User = user;
        IsValid = true;
        Messages = [];
    }
    
    public bool IsValid { get; }
    public IEnumerable<string> Messages { get; }
    public ApplicationUser? User { get; }

    public static ApiKeyValidationResult Failed(IEnumerable<string> messages)
    {
        return new ApiKeyValidationResult(isValid: false, messages);
    }

    public static ApiKeyValidationResult Success(ApplicationUser user)
    {
        return new ApiKeyValidationResult(user);
    }
}