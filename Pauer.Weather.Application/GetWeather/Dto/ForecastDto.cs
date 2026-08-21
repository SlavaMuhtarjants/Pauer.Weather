namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record ForecastDto(
    DayForecastDto? Today,
    DayForecastDto? Tomorrow,
    IReadOnlyList<DaySummaryDto> ThreeDay);