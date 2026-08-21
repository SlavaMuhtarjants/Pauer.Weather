using MediatR;

using Pauer.Weather.Application.Common;
using Pauer.Weather.Application.GetWeather.Dto;

namespace Pauer.Weather.Application.GetWeather;

public sealed record GetWeatherQuery : IRequest<Result<WeatherDto>>;