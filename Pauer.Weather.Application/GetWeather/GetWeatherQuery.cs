using MediatR;

using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Application.GetWeather;

public sealed record GetWeatherQuery(int Days = 3) : IRequest<Result<WeatherDto>>;