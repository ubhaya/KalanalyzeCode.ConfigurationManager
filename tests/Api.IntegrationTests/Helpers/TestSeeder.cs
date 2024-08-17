using KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

namespace KalanalyzeCode.ConfigurationManager.Api.IntegrationTests.Helpers;

public class TestSeeder : IDatabaseSeeder
{
    public Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SeedSampleDataAsync(CancellationToken cancellationToken = default)
    { 
        return Task.CompletedTask;
    }
}