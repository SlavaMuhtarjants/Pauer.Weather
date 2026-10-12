using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Countries.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Infrastructure.Persistence;

internal sealed class CountryRepository(WeatherForecastDbContext dbContext, ILogger<CountryRepository> logger)
    : ICountryRepository
{
    public async Task<Result<IReadOnlyCollection<CountryDto>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var countries = await dbContext.Countries
                .AsNoTracking()
                .OrderBy(country => country.Name)
                .Select(country => new CountryDto(country.Id, country.Name))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result<IReadOnlyCollection<CountryDto>>.Success(countries);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            logger.LogError(exception, "Failed to load countries from the database.");

            return Result<IReadOnlyCollection<CountryDto>>.Failure("Failed to load countries.");
        }
    }
}