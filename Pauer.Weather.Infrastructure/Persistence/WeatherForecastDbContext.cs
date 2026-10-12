using Microsoft.EntityFrameworkCore;

using Pauer.Weather.Domain.Entities;

namespace Pauer.Weather.Infrastructure.Persistence;

public sealed class WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherForecastDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}