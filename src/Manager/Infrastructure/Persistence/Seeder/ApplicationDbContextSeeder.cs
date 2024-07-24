using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Domain.Concrete;
using KalanalyzeCode.ConfigurationManager.Infrastructure.Identity;
using KalanalyzeCode.ConfigurationManager.Shared.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;

internal class ApplicationDbContextSeeder : IDatabaseSeeder
{
    private readonly ILogger<ApplicationDbContextSeeder> _logger;
    private readonly ApplicationDbContext _context;
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ConfigurationDbContext _configurationDbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly List<ConfigurationSettings> _configurationSettingsList =
    [
        new ConfigurationSettings
        {
            Id = "StarfishOptions:FraudCheckerEnabled",
            Value = "true"
        },
        new ConfigurationSettings
        {
            Id = "StarfishOptions:PerformanceMonitorEnabled",
            Value = "true"
        },
    ];
    
    private const string? AdministratorsRole = "Administrators";
    private const string? AccountsRole = "Accounts";
    private const string? OperationsRole = "Operations";

    private const string DefaultPassword = "Password123!";

    public ApplicationDbContextSeeder(ILogger<ApplicationDbContextSeeder> logger,
        ApplicationDbContext context, PersistedGrantDbContext persistedGrantDbContext,
        ConfigurationDbContext configurationDbContext, UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _persistedGrantDbContext = persistedGrantDbContext;
        _configurationDbContext = configurationDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        await InitialiseWithMigrationsAsync();
    }

    public async Task SeedAsync()
    {
        await SeedIdentityAsync();
        await SeedSampleDataAsync();
    }

    private async Task SeedIdentityAsync()
    {
        await CreateRole(AdministratorsRole, Permissions.All);
        await CreateRole(AccountsRole, Permissions.ViewUsers | Permissions.Counter);
        await CreateRole(OperationsRole, Permissions.ViewUsers | Permissions.Forecast);

        await CreateUser("admin@localhost", new[] { AdministratorsRole });
        await CreateUser("auditor@localost", new[] { AccountsRole, OperationsRole });

        await _context.SaveChangesAsync();
    }

    private async Task CreateRole(string? roleName, Permissions permissions)
    {
        await _roleManager.CreateAsync(
            new ApplicationRole { Name = roleName, NormalizedName = roleName?.ToUpper(), Permissions = permissions });
    }

    private async Task CreateUser(string userName, IEnumerable<string?>? roles = null)
    {
        var user = new ApplicationUser { UserName = userName, Email = userName };

        await _userManager.CreateAsync(user, DefaultPassword);

        user = await _userManager.FindByNameAsync(userName);

        foreach (var role in roles ?? [])
        {
            if (!string.IsNullOrEmpty(role))
                await _userManager.AddToRoleAsync(user!, role);
        }
    }

    public async Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    {
        if (!_context.Settings.Any())
        {
            await _context.Settings.AddRangeAsync(_configurationSettingsList, cancellationToken);
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, _configurationSettingsList.Count);
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSaved, DateOnly.FromDateTime(DateTime.UtcNow),
            TimeOnly.FromDateTime(DateTime.UtcNow));
        
        await _persistedGrantDbContext.Database.MigrateAsync(cancellationToken: cancellationToken);

        await _configurationDbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        
        if (!_configurationDbContext.Clients.Any())
        {
            foreach (var client in Config.Clients())
            {
                _configurationDbContext.Clients.Add(client.ToEntity());
            }
            await _configurationDbContext.SaveChangesAsync(cancellationToken);
        }

        if (!_configurationDbContext.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                _configurationDbContext.IdentityResources.Add(resource.ToEntity());
            }
            await _configurationDbContext.SaveChangesAsync(cancellationToken);
        }

        if (!_configurationDbContext.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                _configurationDbContext.ApiScopes.Add(resource.ToEntity());
            }
            await _configurationDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task InitialiseWithMigrationsAsync()
    {
        if (_context.Database.IsNpgsql())
        {
            await _context.Database.MigrateAsync();
        }
        else
        {
            await _context.Database.EnsureCreatedAsync();
        }
    }
}