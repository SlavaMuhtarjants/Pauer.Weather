using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiDaySummaryResponse(
    [property: JsonPropertyName("maxtemp_c")] double MaxTempC,
    [property: JsonPropertyName("mintemp_c")] double MinTempC,
    [property: JsonPropertyName("condition")] WeatherApiConditionResponse Condition);