using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Infrastructure.Contracts;

namespace Pauer.Weather.Infrastructure.Mapping;

internal static class GeoApiMapper
{
    public static IReadOnlyCollection<CityDto> ToCityDtos(GeoCityListResponse response) =>
        (response.Items ?? [])
        // Settlements without coordinates cannot be used to request a forecast.
        .Where(city => city.Latitude.HasValue && city.Longitude.HasValue)
        .Select(city => new CityDto(
            city.Id,
            ToDisplayName(city),
            city.FullName ?? ToDisplayName(city),
            city.Latitude!.Value,
            city.Longitude!.Value))
        .ToArray();

    private static string ToDisplayName(GeoCityResponse city) =>
        string.IsNullOrWhiteSpace(city.Name) ? city.English ?? string.Empty : city.Name;
}