namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record WeatherDto(CurrentWeatherDto CurrentWeather, ForecastDto Forecast);