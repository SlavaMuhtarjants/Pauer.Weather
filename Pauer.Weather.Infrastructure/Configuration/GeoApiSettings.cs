namespace Pauer.Weather.Infrastructure.Configuration;

public sealed class GeoApiSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
}