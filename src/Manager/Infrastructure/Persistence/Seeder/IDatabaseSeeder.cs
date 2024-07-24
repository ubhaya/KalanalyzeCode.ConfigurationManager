namespace KalanalyzeCode.ConfigurationManager.Infrastructure.Persistence.Seeder;

public interface IDatabaseSeeder
{
    Task InitializeAsync();
    Task SeedAsync();
}