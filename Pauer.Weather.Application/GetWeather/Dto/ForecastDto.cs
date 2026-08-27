namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record ForecastDto(
    DayForecastDto? Today,
    DayForecastDto? Tomorrow,
    IReadOnlyCollection<DaySummaryDto> ThreeDay);