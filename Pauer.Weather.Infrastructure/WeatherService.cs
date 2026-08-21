using Microsoft.Extensions.Logging;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Common;
using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Infrastructure.Mapping;

namespace Pauer.Weather.Infrastructure;

internal sealed class WeatherService(WeatherApiHttpClient httpClient, ILogger<WeatherService> logger) : IWeatherService
{
    public async Task<Result<WeatherDto>> GetWeatherAsync(double latitude, double longitude, CancellationToken cancellationToken)
    {
        try
        {
            var currentWeather = httpClient.GetCurrentWeatherAsync(latitude, longitude, cancellationToken);
            var forecastWeather = httpClient.GetForecastAsync(latitude, longitude, cancellationToken);

            await Task.WhenAll(currentWeather, forecastWeather)
                .ConfigureAwait(false);

            var dto = WeatherApiMapper.ToWeatherDto(currentWeather.Result, forecastWeather.Result);

            return Result<WeatherDto>.Success(dto);
        }
        catch (Exception exception)
        {
            if (exception is not OperationCanceledException or not TaskCanceledException)
            {
                logger.LogError(exception, exception.Message);
            }

            if (exception is HttpRequestException)
            {
                logger.LogWarning(exception, exception.Message);
            }
            
            return Result<WeatherDto>.Failure("Failed to retrieve weather data.");
        }
    }
}