using IdentityServer.Data;
using IdentityServer.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Services.Interfaces;

public sealed class ApiKeyValidatorService : IApiKeyValidatorService
{
    private readonly ApplicationDbContext _context;
    
    public ApiKeyValidatorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiKeyValidationResult> Validate(string apiKey, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.ApiKey == apiKey, cancellationToken);
        
        if (user is null)
            return ApiKeyValidationResult.Failed(["Invalid Api Key"]);
        
        // Todo: If neccessory do more validation
        
        return ApiKeyValidationResult.Success(user);
    }
}