using Microsoft.Extensions.Options;

using Pauer.Weather.Domain.ValueObjects;
using Pauer.Weather.Infrastructure.Configuration;
using Pauer.Weather.Infrastructure.Contracts;

using System.Globalization;
using System.Net.Http.Json;

namespace Pauer.Weather.Infrastructure;

internal sealed class WeatherApiHttpClient(HttpClient httpClient, IOptions<WeatherApiSettings> options)
{
    public async Task<WeatherApiForecastResponse> GetForecastAsync(
        Coordinates coordinates,
        ForecastDays days,
        CancellationToken cancellationToken)
    {
        var url = $"forecast.json?key={options.Value.ApiKey}&q={FormatCoordinate(coordinates.Latitude)},{FormatCoordinate(coordinates.Longitude)}&days={days.Value}";
        
        var response = await httpClient.GetFromJsonAsync<WeatherApiForecastResponse>(url, cancellationToken)
            .ConfigureAwait(false);
        
        return response ?? throw new InvalidOperationException("Empty response from WeatherAPI forecast endpoint.");
    }

    private static string FormatCoordinate(double value) => value.ToString(CultureInfo.InvariantCulture);
}