using KalanalyzeCode.ConfigurationManager.Ui.GrantValidators;

namespace KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;

internal interface IApiKeyValidatorService
{
    Task<ApiKeyValidationResult> Validate(Guid apiKey, CancellationToken cancellationToken = default);
}