namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record DayForecastDto(DateOnly Date, IReadOnlyCollection<HourlyForecastDto> Hours);