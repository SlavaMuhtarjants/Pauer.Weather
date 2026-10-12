using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pauer.Weather.Domain.Entities;

namespace Pauer.Weather.Infrastructure.Persistence.Configurations;

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("country");

        builder.HasKey(country => country.Id);

        builder.Property(country => country.Id)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(country => country.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(country => country.Name);
    }
}