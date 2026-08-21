using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiHourResponse(
    [property: JsonPropertyName("time")] string Time,
    [property: JsonPropertyName("temp_c")] double TempC,
    [property: JsonPropertyName("condition")] WeatherApiConditionResponse Condition);