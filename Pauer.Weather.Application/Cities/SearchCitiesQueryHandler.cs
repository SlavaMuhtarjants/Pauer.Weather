using MediatR;

using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

namespace Pauer.Weather.Application.Cities;

public sealed class SearchCitiesQueryHandler(ICitySearchService citySearchService)
    : IRequestHandler<SearchCitiesQuery, Result<IReadOnlyCollection<CityDto>>>
{
    public async Task<Result<IReadOnlyCollection<CityDto>>> Handle(
        SearchCitiesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CountryId))
        {
            return Result<IReadOnlyCollection<CityDto>>.Failure("Country must be selected.");
        }

        var searchTermResult = CitySearchTerm.Create(request.SearchTerm);

        if (!searchTermResult.IsSuccess)
        {
            return Result<IReadOnlyCollection<CityDto>>.Failure(searchTermResult.Error!);
        }

        var response = await citySearchService
            .SearchAsync(request.CountryId, searchTermResult.Value, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }
}