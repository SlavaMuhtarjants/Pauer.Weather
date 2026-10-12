using MediatR;

using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

namespace Pauer.Weather.Application.GetWeather;

public sealed class GetWeatherQueryHandler(IWeatherService weatherService)
    : IRequestHandler<GetWeatherQuery, Result<WeatherDto>>
{
    public async Task<Result<WeatherDto>> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        var coordinatesResult = Coordinates.Create(request.Latitude, request.Longitude);

        if (!coordinatesResult.IsSuccess)
        {
            return Result<WeatherDto>.Failure(coordinatesResult.Error!);
        }

        var daysResult = ForecastDays.Create(request.Days);

        if (!daysResult.IsSuccess)
        {
            return Result<WeatherDto>.Failure(daysResult.Error!);
        }

        var response = await weatherService.GetWeatherAsync(coordinatesResult.Value, daysResult.Value, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }
}