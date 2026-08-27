using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record WeatherApiForecastContainerResponse(
    [property: JsonPropertyName("forecastday")] IReadOnlyCollection<WeatherApiForecastDayResponse> ForecastDay);