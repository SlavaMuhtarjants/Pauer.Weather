using Pauer.Weather.Application.Common;
using Pauer.Weather.Application.GetWeather.Dto;

namespace Pauer.Weather.Application;

public interface IWeatherService
{
    Task<Result<WeatherDto>> GetWeatherAsync(Coordinates coordinates, CancellationToken cancellationToken);
}