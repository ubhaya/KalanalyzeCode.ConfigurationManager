using KalanalyzeCode.ConfigurationManager.Application.Helpers;
using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using KalanalyzeCode.ConfigurationManager.Entity.Entities;
using Microsoft.Extensions.Logging;

namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

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

    private readonly List<Project> _projectList =
    [
        new Project {Id = Guid.NewGuid(), Name = "Project 1", ApiKey = Guid.NewGuid()},
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
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, nameof(_context.Settings),
                _configurationSettingsList.Count);
        }

        if (!_context.Projects.Any())
        {
            await _context.Projects.AddRangeAsync(_projectList, cancellationToken);
            _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSeed, nameof(_context.Projects),
                _projectList.Count);
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(AppConstants.LoggingMessages.DatabaseSaved, DateOnly.FromDateTime(DateTime.UtcNow),
            TimeOnly.FromDateTime(DateTime.UtcNow));
    }
}