using MediatR;

using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Application.Cities;

public sealed record SearchCitiesQuery(string CountryId, string SearchTerm)
    : IRequest<Result<IReadOnlyCollection<CityDto>>>;