using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using KalanalyzeCode.ConfigurationManager.Ui.GrantValidators;
using Microsoft.EntityFrameworkCore;

namespace KalanalyzeCode.ConfigurationManager.Ui.Services.Interfaces;

internal sealed class ApiKeyValidatorService : IApiKeyValidatorService
{
    private readonly ApplicationDbContext _context;
    
    public ApiKeyValidatorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiKeyValidationResult> Validate(Guid apiKey, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == apiKey, cancellationToken);
        
        if (user is null)
            return ApiKeyValidationResult.Failed(["Invalid Api Key"]);
        
        // Todo: If neccessory do more validation
        
        return ApiKeyValidationResult.Success(user);
    }
}