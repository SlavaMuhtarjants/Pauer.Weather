using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiForecastResponse(
    [property: JsonPropertyName("location")] WeatherApiLocationResponse Location,
    [property: JsonPropertyName("current")] WeatherApiCurrentConditionsResponse Current,
    [property: JsonPropertyName("forecast")] WeatherApiForecastContainerResponse Forecast);