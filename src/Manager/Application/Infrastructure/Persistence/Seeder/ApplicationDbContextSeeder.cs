using System.Security.Claims;
using IdentityModel;
using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Common.Models;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Logging;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

public class ApplicationDbContextSeeder : IDatabaseSeeder
{
    private readonly ILogger<ApplicationDbContextSeeder> _logger;
    private readonly IApplicationDbContext _context;
    private readonly List<ConfigurationSettings> _configurationSettingsList =
    [
        new ConfigurationSettings{
            Id = "StarfishOptions:FraudCheckerEnabled", 
            Value = "true"
        },
        new ConfigurationSettings{
            Id = "StarfishOptions:PerformanceMonitorEnabled", 
            Value = "true"
        },
    ];
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly Dictionary<string, TestUser> _seedUsers = new()
    {
        {
            "alice",
            new TestUser("AliceSmith@email.com",
            [
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alice.com")

            ], [], string.Empty)
        },
        {
            "bob",
            new TestUser("BobSmith@email.com", [
                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                new Claim("location", "somewhere"),
            ], ["Administrator"], string.Empty)
        },
        {
            "apiKeyUser",
            new TestUser("apiKeyUser@email.com", [
            ], ["ApiKey"], "674e2a077bd64d669340af69c460767c")
        }
    };

    public static readonly Dictionary<string, Permissions> Roles = new()
    {
        { "Administrator", Permissions.All },
        { "ApiKey", Permissions.AppSettingsRead }
    };

    private static readonly Guid ProjectOneId = Guid.NewGuid();
    
    private readonly List<Project> _projectList =
    [
        new Project {Id = ProjectOneId, Name = "Project 1", ApiKey = Guid.NewGuid()},
        new Project {Id = Guid.NewGuid(), Name = "Project 2", ApiKey = Guid.NewGuid()},
        new Project {Id = Guid.NewGuid(), Name = "Project 3"},
        new Project {Id = Guid.NewGuid(), Name = "Project 4"},
        new Project {Id = Guid.NewGuid(), Name = "Project 5", ApiKey = Guid.NewGuid()},
        new Project {Id = Guid.NewGuid(), Name = "Project 6"},
        new Project {Id = Guid.NewGuid(), Name = "Project 7"},
        new Project {Id = Guid.NewGuid(), Name = "Project 8"},
        new Project {Id = Guid.NewGuid(), Name = "Project 9"},
        new Project {Id = Guid.NewGuid(), Name = "Project 10"},
        new Project {Id = Guid.NewGuid(), Name = "Project 11"},
        new Project {Id = Guid.NewGuid(), Name = "Project 12"},
        new Project {Id = Guid.NewGuid(), Name = "Project 13"},
        new Project {Id = Guid.NewGuid(), Name = "Project 14"},
        new Project {Id = Guid.NewGuid(), Name = "Project 15"},
    ];

    private readonly List<Configuration> _configurations =
    [
        new Configuration { Name = "PostgreSql__ConnectionString", Value = "TestString", ProjectId = ProjectOneId },
        new Configuration { Name = "PostgreSql__DbPassword", Value = "Password", ProjectId = ProjectOneId },
    ];
    
    private const string DefaultPassword = "Pass123$";

    public ApplicationDbContextSeeder(ILogger<ApplicationDbContextSeeder> logger, 
        IApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        foreach (var (roleName, permissions) in Roles)
        {
            await EnsureRoles(roleName, permissions);
        }

        foreach (var (userName, data) in _seedUsers)
        {
            await EnsureUser(userName, data);
        }

        await SeedSampleDataAsync(cancellationToken);
    }
    
    public async Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    {
        if (!_context.Settings.Any())
        {
            await _context.Settings.AddRangeAsync(_configurationSettingsList, cancellationToken);
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, nameof(_context.Settings),
                _configurationSettingsList.Count);
        }

        if (!_context.Projects.Any())
        {
            await _context.Projects.AddRangeAsync(_projectList, cancellationToken);
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, nameof(_context.Projects),
                _projectList.Count);
        }

        if (!_context.Configurations.Any())
        {
            await _context.Configurations.AddRangeAsync(_configurations, cancellationToken);
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, nameof(_context.Configurations),
                _configurations.Count);
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSaved, DateOnly.FromDateTime(DateTime.UtcNow),
            TimeOnly.FromDateTime(DateTime.UtcNow));
    }
    
    private async Task EnsureRoles(string roleName, Permissions permissions)
    {
        var isExists = await _roleManager.RoleExistsAsync(roleName);

        if (!isExists)
        {
            var role = new ApplicationRole
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

    private async Task EnsureUser(string userName, TestUser data)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = data.Email,
                EmailConfirmed = true,
                //ApiKey = data.ApiKey
            };
            var result = await _userManager.CreateAsync(user, DefaultPassword);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await _userManager.AddClaimsAsync(user, data.Claims);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            _logger.LogUserCreated(user.UserName);

            await AddToRole(user, data.Roles);
        }
        else
        {
            _logger.LogUserAlreadyExists(user.UserName!);
            await AddToRole(user, data.Roles);
        }
    }

    private async Task AddToRole(ApplicationUser user, IEnumerable<string> roles)
    {
        foreach (var roleName in roles)
        {
            var result = await _userManager.IsInRoleAsync(user, roleName);
            if (result)
            {
                _logger.LogUserAlreadyInRole(user.UserName!, roleName);
                return;
            }
            var identityResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.First().Description);
            }

            _logger.LogUserAddToRole(user.UserName!, roleName);
        }
    }
    
    internal record TestUser(string Email, IEnumerable<Claim> Claims, IEnumerable<string> Roles, string ApiKey);
}