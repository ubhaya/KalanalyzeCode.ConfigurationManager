namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Seeder;

public interface IDatabaseSeeder
{
    Task SeedSampleDataAsync(CancellationToken cancellationToken = default);
}