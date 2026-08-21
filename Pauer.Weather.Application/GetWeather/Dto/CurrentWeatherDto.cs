namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record CurrentWeatherDto(
    string LocationName,
    DateTime LocalDateTime,
    double Temperature,
    double FeelsLike,
    int HumidityPercent,
    double WindKph,
    string ConditionText,
    string ConditionIconUrl);