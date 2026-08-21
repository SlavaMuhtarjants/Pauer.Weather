using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiCurrentResponse(
    [property: JsonPropertyName("location")] WeatherApiLocationResponse Location,
    [property: JsonPropertyName("current")] WeatherApiCurrentConditionsResponse Current);