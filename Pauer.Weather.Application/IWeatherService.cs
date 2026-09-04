using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

namespace Pauer.Weather.Application;

public interface IWeatherService
{
    Task<Result<WeatherDto>> GetWeatherAsync(
        Coordinates coordinates,
        ForecastDays forecastDays,
        CancellationToken cancellationToken);
}