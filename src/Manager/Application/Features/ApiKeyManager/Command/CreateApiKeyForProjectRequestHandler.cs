using System.Net.Http.Json;
using KalanalyzeCode.ConfigurationManager.Application.Authorization;
using KalanalyzeCode.ConfigurationManager.Application.Common.Models;
using KalanalyzeCode.ConfigurationManager.Application.Contract.Request.ApiKeyManager;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence;
using LanguageExt;
using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KalanalyzeCode.ConfigurationManager.Application.Features.ApiKeyManager.Command;

public sealed class CreateApiKeyForProjectRequestHandler : IRequestHandler<CreateApiKeyForProjectRequest, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly PasswordOptions _passwordOptions;

    public CreateApiKeyForProjectRequestHandler(IApplicationDbContext context,  IOptions<PasswordOptions> passwordOptions,
        UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordOptions = passwordOptions.Value;
    }

    public async Task<Result<Guid>> Handle(CreateApiKeyForProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync([request.ProjectId], cancellationToken);
        
        if (project is null)
            return new Result<Guid>(new Exception($"Project {request.ProjectId} not found"));

        if (project.ApiKey != Guid.Empty)
        {
            return project.ApiKey;
        }

        var newApiKey = Guid.NewGuid();

        var result = await CreateNewApiKey(newApiKey, cancellationToken);

        return await result.Match<Task<Result<Guid>>>(async g =>
        {
            project.ApiKey = g;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);
            return newApiKey;
        }, exception => Task.FromResult(new Result<Guid>(exception)));
    }

    private async Task<Result<Guid>> CreateNewApiKey(Guid newKey, CancellationToken cancellationToken = default)
    {
        var isApiKeyExists =
            await _context.Users.SingleOrDefaultAsync(u => u.Id == newKey, cancellationToken);

        if (isApiKeyExists is not null)
            return new Result<Guid>(new Exception("Api Key is already in database"));

        var apiKeyUser = new ApplicationUser()
        {
            Id = newKey,
            UserName = newKey.ToString("N"),
            Email = $"{newKey:N}@{newKey:N}.{newKey:N}",
            EmailConfirmed = true
        };

        var identityResult = await _userManager.CreateAsync(apiKeyUser, Password.Generate(_passwordOptions));

        if (!identityResult.Succeeded)
            return new Result<Guid>(new Exception(identityResult.Errors.First().Description));

        await VeryfingRoleExists(); 

        identityResult = await _userManager.AddToRoleAsync(apiKeyUser, "ApiKey");

        if (!identityResult.Succeeded)
            return new Result<Guid>(new Exception(identityResult.Errors.First().Description));

        return newKey;
    }

    private async Task VeryfingRoleExists()
    {
        var isExists = await _roleManager.RoleExistsAsync("ApiKey");
        if (!isExists)
        {
            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "ApiKey",
                Permissions = Permissions.AppSettingsRead
            });
        }
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