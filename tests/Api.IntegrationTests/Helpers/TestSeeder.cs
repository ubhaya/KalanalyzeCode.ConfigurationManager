using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class TestSeeder : IDatabaseSeeder
{
    public Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    { 
        return Task.CompletedTask;
    }
}