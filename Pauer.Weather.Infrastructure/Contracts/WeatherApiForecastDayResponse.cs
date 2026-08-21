using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiForecastDayResponse(
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("day")] WeatherApiDaySummaryResponse Day,
    [property: JsonPropertyName("hour")] IReadOnlyList<WeatherApiHourResponse> Hour);