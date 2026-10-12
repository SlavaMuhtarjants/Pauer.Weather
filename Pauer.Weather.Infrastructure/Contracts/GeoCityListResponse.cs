using System.Text.Json.Serialization;

namespace Pauer.Weather.Infrastructure.Contracts;

internal sealed record GeoCityListResponse
{
    [JsonPropertyName("status")]
    public int Status { get; init; }

    [JsonPropertyName("items")]
    public IReadOnlyCollection<GeoCityResponse>? Items { get; init; }
}