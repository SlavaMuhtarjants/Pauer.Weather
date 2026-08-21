using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiLocationResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("localtime")] string LocalTime);