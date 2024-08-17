namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

public interface IDatabaseSeeder
{
    Task SeedDataAsync(CancellationToken cancellationToken = default);
    Task SeedSampleDataAsync(CancellationToken cancellationToken = default);
}