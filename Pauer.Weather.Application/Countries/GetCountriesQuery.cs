using MediatR;

using Pauer.Weather.Application.Countries.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Application.Countries;

public sealed record GetCountriesQuery : IRequest<Result<IReadOnlyCollection<CountryDto>>>;