using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiCurrentConditionsResponse(
    [property: JsonPropertyName("temp_c")] double TempC,
    [property: JsonPropertyName("feelslike_c")] double FeelsLikeC,
    [property: JsonPropertyName("humidity")] int Humidity,
    [property: JsonPropertyName("wind_kph")] double WindKph,
    [property: JsonPropertyName("condition")] WeatherApiConditionResponse Condition);