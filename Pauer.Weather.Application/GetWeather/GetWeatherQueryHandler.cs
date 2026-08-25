using MediatR;

using Microsoft.Extensions.Options;

using Pauer.Weather.Application.Common;
using Pauer.Weather.Application.Configuration;
using Pauer.Weather.Application.GetWeather.Dto;

namespace Pauer.Weather.Application.GetWeather;

public sealed class GetWeatherQueryHandler(IWeatherService weatherService, IOptions<WeatherLocationSettings> locationOptions)
    : IRequestHandler<GetWeatherQuery, Result<WeatherDto>>
{
    public async Task<Result<WeatherDto>> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        var location = locationOptions.Value;
        
        var coordinatesResult = Coordinates.Create(location.Latitude, location.Longitude);

        if (!coordinatesResult.IsSuccess)
        {
            return Result<WeatherDto>.Failure(coordinatesResult.Error!);
        }

        var response = await weatherService.GetWeatherAsync(coordinatesResult.Value, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }
}