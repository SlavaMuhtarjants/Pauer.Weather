using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

namespace Pauer.Weather.Application;

public interface ICitySearchService
{
    Task<Result<IReadOnlyCollection<CityDto>>> SearchAsync(
        string countryId,
        CitySearchTerm searchTerm,
        CancellationToken cancellationToken);
}