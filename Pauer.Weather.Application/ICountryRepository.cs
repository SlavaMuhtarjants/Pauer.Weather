using Pauer.Weather.Application.Countries.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Application;

public interface ICountryRepository
{
    Task<Result<IReadOnlyCollection<CountryDto>>> GetAllAsync(CancellationToken cancellationToken);
}