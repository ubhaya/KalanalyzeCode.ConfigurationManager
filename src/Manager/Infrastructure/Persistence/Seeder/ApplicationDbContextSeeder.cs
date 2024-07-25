using KalanalyzeCode.ConfigurationManager.Application.Common.Interfaces;
using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Domain.Concrete;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;

internal class ApplicationDbContextSeeder : IDatabaseSeeder
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

    public ApplicationDbContextSeeder(ILogger<ApplicationDbContextSeeder> logger, 
        IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
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
    }
}