namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record HourlyForecastDto(DateTime Time, double Temperature, string ConditionIconUrl);