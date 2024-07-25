namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;

public interface IDatabaseSeeder
{
    Task SeedSampleDataAsync(CancellationToken cancellationToken = default);
}