using System.Security.Claims;
using IdentityModel;
using KalanalyzeCode.ConfigurationManager.Ui.Data;
using KalanalyzeCode.ConfigurationManager.Ui.Logging;
using KalanalyzeCode.ConfigurationManager.Ui.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data;

public class ApplicationDbContextSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<ApplicationDbContextSeeder> _logger;

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

    // private readonly Dictionary<string, Permissions> _roles = new()
    // {
    //     { "Administrator", Permissions.All },
    //     { "ApiKey", Permissions.AppSettingsRead }
    // };

    private readonly IEnumerable<string> _roles =
    [
        "Administrator",
        "ApiKey"
    ];
    
    private const string DefaultPassword = "Pass123$";

    public ApplicationDbContextSeeder(ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        ILogger<ApplicationDbContextSeeder> logger) 
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedDataAsync()
    {
        // foreach (var (roleName, permissions) in _roles)
        // {
        //     await EnsureRoles(roleName, permissions);
        // }
        
        foreach (var roleName in _roles)
        {
            await EnsureRoles(roleName);
        }

        foreach (var (userName, data) in _seedUsers)
        {
            await EnsureUser(userName, data);
        }
    }

    // private async Task EnsureRoles(string roleName, Permissions permissions)
    private async Task EnsureRoles(string roleName)
    {
        var isExists = await _roleManager.RoleExistsAsync(roleName);

        if (!isExists)
        {
            var role = new ApplicationRole()
            {
                Name = roleName,
                //Permissions = permissions
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