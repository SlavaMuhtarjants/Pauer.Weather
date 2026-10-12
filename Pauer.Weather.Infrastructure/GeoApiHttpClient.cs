using Microsoft.Extensions.Options;

using Pauer.Weather.Domain.ValueObjects;
using Pauer.Weather.Infrastructure.Configuration;
using Pauer.Weather.Infrastructure.Contracts;

using System.Net.Http.Json;

namespace Pauer.Weather.Infrastructure;

internal sealed class GeoApiHttpClient(HttpClient httpClient, IOptions<GeoApiSettings> options)
{
    public async Task<GeoCityListResponse> GetCitiesAsync(
        string countryId,
        CitySearchTerm searchTerm,
        CancellationToken cancellationToken)
    {
        var url = $"json/geo/city_list?country={Uri.EscapeDataString(countryId)}&q={Uri.EscapeDataString(searchTerm.Value)}&api_key={options.Value.ApiKey}";

        var response = await httpClient.GetFromJsonAsync<GeoCityListResponse>(url, cancellationToken)
            .ConfigureAwait(false);

        return response ?? throw new InvalidOperationException("Empty response from geo API city list endpoint.");
    }
}