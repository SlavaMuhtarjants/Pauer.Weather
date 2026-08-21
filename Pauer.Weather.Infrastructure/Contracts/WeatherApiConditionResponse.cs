using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiConditionResponse(
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("icon")] string Icon);