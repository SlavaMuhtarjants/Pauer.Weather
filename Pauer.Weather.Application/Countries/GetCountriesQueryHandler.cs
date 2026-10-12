using MediatR;

using Pauer.Weather.Application.Countries.Dto;
using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Application.Countries;

public sealed class GetCountriesQueryHandler(ICountryRepository countryRepository)
    : IRequestHandler<GetCountriesQuery, Result<IReadOnlyCollection<CountryDto>>>
{
    public async Task<Result<IReadOnlyCollection<CountryDto>>> Handle(
        GetCountriesQuery request,
        CancellationToken cancellationToken)
    {
        return await countryRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
    }
}