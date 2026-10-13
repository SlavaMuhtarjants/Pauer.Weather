using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;
using Pauer.Weather.Infrastructure.Mapping;

namespace Pauer.Weather.Infrastructure;

internal sealed class CitySearchService(
    GeoApiHttpClient httpClient,
    IMemoryCache cache,
    ILogger<CitySearchService> logger) : ICitySearchService
{
    private static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(10);

    public async Task<Result<IReadOnlyCollection<CityDto>>> SearchAsync(
        string countryId,
        CitySearchTerm searchTerm,
        CancellationToken cancellationToken)
    {
        var cacheKey = BuildCacheKey(countryId, searchTerm);

        if (cache.TryGetValue(cacheKey, out IReadOnlyCollection<CityDto>? cached) && cached is not null)
        {
            return Result<IReadOnlyCollection<CityDto>>.Success(cached);
        }

        try
        {
            var response = await httpClient.GetCitiesAsync(countryId, searchTerm, cancellationToken)
                .ConfigureAwait(false);

            var cities = GeoApiMapper.ToCityDtos(response);

            cache.Set(cacheKey, cities, CacheLifetime);

            return Result<IReadOnlyCollection<CityDto>>.Success(cities);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            if (exception is HttpRequestException)
            {
                logger.LogWarning(exception, exception.Message);
            }
            else
            {
                logger.LogError(exception, exception.Message);
            }

            return Result<IReadOnlyCollection<CityDto>>.Failure("Failed to search for cities.");
        }
    }

    private static string BuildCacheKey(string countryId, CitySearchTerm searchTerm) =>
        $"city-search:{countryId.ToUpperInvariant()}:{searchTerm.Value.ToLowerInvariant()}";
}