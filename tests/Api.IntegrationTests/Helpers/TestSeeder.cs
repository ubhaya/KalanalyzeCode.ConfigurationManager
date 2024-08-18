using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Common.Models;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;
using KalanalyzeCode.ConfigurationManager.Application.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class TestSeeder : IDatabaseSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<TestSeeder> _logger;

    public TestSeeder(ILogger<TestSeeder> logger, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        foreach (var (roleName, permissions) in ApplicationDbContextSeeder.Roles)
        {
            await EnsureRoles(roleName, permissions);
        }
    }

    public Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    { 
        return Task.CompletedTask;
    }
    
    private async Task EnsureRoles(string roleName, Permissions permissions)
    {
        var isExists = await _roleManager.RoleExistsAsync(roleName);

        if (!isExists)
        {
            var role = new ApplicationRole()
            {
                Name = roleName,
                Permissions = permissions
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            _logger.LogRoleCreated(roleName);
        }
        else
        {
            _logger.LogRoleAlreadyExists(roleName);
        }
    }
}