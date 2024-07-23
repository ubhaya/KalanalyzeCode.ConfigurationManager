using KalanalyzeCode.ConfigurationManager.Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ConfigurationSettings>
{
    public void Configure(EntityTypeBuilder<ConfigurationSettings> builder)
    {
        builder.Ignore(x => x.DomainEvents);
    }
}