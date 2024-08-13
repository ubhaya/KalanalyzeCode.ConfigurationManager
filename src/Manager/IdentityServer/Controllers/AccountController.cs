using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityServer.Controllers;

[Route("api/[controller]")]
public sealed class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly PasswordOptions _passwordOptions;

    public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
        IOptions<PasswordOptions> passwordOptions)
    {
        _userManager = userManager;
        _context = context;
        _passwordOptions = passwordOptions.Value;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var isApiKeyExists =
            await _context.Users.SingleOrDefaultAsync(u => u.ApiKey == request.ApiKey, cancellationToken);

        if (isApiKeyExists is not null)
            return BadRequest("Api Key is already in database");

        var apiKeyUser = new ApplicationUser()
        {
            ApiKey = request.ApiKey,
            UserName = request.ApiKey,
            Email = $"{request.ApiKey}@{request.ApiKey}.{request.ApiKey}",
            EmailConfirmed = true
        };

        var identityResult = await _userManager.CreateAsync(apiKeyUser, Password.Generate(_passwordOptions));

        if (!identityResult.Succeeded)
            return BadRequest(identityResult.Errors.Select(e => e.Description));

        identityResult = await _userManager.AddToRoleAsync(apiKeyUser, "ApiKey");

        if (!identityResult.Succeeded)
            return BadRequest(identityResult.Errors.Select(e => e.Description));

        return Ok(apiKeyUser);
    }
}

public static class Password
{
    private static readonly string[] RandomChars = new[]
    {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ", // Uppercase 
        "abcdefghijkmnopqrstuvwxyz", // Lowercase
        "0123456789", // Digits
        "!@$?_-" // Special Characters
    };

    public static string Generate(PasswordOptions passwordOptions)
    {
        var random = new Random(Environment.TickCount);

        List<char> chars = [];
        
        if (passwordOptions.RequireUppercase)
        {
            var upperCase = RandomChars[0];
            chars.Insert(random.Next(0, chars.Count), upperCase[random.Next(0, upperCase.Length)]);
        }

        if (passwordOptions.RequireLowercase)
        {
            var lowerCase = RandomChars[1];
            chars.Insert(random.Next(0, chars.Count), lowerCase[random.Next(0, lowerCase.Length)]);
        }
        
        if (passwordOptions.RequireDigit)
        {
            var digits = RandomChars[2];
            chars.Insert(random.Next(0, chars.Count), digits[random.Next(0, digits.Length)]);
        }

        if (passwordOptions.RequireNonAlphanumeric)
        {
            var special = RandomChars[3];
            chars.Insert(random.Next(0, chars.Count), special[random.Next(0, special.Length)]);
        }

        while (chars.Count<passwordOptions.RequiredLength)
        {
            var rcs = RandomChars[random.Next(0, RandomChars.Length)];
            chars.Insert(random.Next(0, chars.Count), rcs[random.Next(0, rcs.Length)]);
        }

        return new string(chars.ToArray());
    }
}