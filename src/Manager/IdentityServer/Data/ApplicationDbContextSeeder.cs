using System.Security.Claims;
using Identity.Shared.Authorization;
using IdentityModel;
using IdentityServer.Infrastructure.Identity;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Data;

public class ApplicationDbContextSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<ApplicationDbContextSeeder> _logger;

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
        await EnsureRoles("Administrator");
        
        await EnsureUser("alice", "AliceSmith@email.com", "Alice Smith", 
            "Alice", "Smith", "http://alice.com");
        await EnsureUser("bob", "BobSmith@email.com", "Bob Smith", 
            "Bob", "Smith", "http://bob.com");
    }

    private async Task EnsureRoles(string roleName)
    {
        var isExists = await _roleManager.RoleExistsAsync(roleName);

        if (!isExists)
        {
            var role = new ApplicationRole()
            {
                Name = roleName,
                Permissions = Permissions.All
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            _logger.LogDebug("{Role} created", roleName);
        }
        else
        {
            _logger.LogDebug("{Role} already exists", roleName);
        }
    }

    private async Task EnsureUser(string userName, string email, string name, string givenName, string familyName,
        string webSite)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, "Pass123$");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await _userManager.AddClaimsAsync(user, new Claim[]{
                new(JwtClaimTypes.Name, name),
                new(JwtClaimTypes.GivenName, givenName),
                new(JwtClaimTypes.FamilyName, familyName),
                new(JwtClaimTypes.WebSite, webSite),
            });
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            
            _logger.LogDebug("{Username} created", userName);

            await AddToRole(user);
        }
        else
        {
            _logger.LogDebug("{Username} already exists", userName);
            await AddToRole(user);
        }
    }

    private async Task AddToRole(ApplicationUser user)
    {
        var result = await _userManager.IsInRoleAsync(user, "Administrator");
        if (result)
        {
            _logger.LogDebug("{User} already in {Role}", user.UserName, "Administrator");
            return;
        }
        var identityResult = await _userManager.AddToRoleAsync(user, "Administrator");
        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.Errors.First().Description);
        }
    }
}