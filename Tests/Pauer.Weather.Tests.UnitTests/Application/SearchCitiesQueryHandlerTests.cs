using FluentAssertions;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Cities;
using Pauer.Weather.Application.Cities.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Application;

public sealed class SearchCitiesQueryHandlerTests
{
    [Fact]
    public async Task Handle_SearchTermTooShort_ReturnsError()
    {
        var citySearchService = new Mock<ICitySearchService>();
        var handler = new SearchCitiesQueryHandler(citySearchService.Object);

        var result = await handler.Handle(new SearchCitiesQuery("RU", "mow"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("longer than 3 characters");
        citySearchService.Verify(
            service => service.SearchAsync(It.IsAny<string>(), It.IsAny<CitySearchTerm>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_CountryIdEmpty_ReturnsError()
    {
        var citySearchService = new Mock<ICitySearchService>();
        var handler = new SearchCitiesQueryHandler(citySearchService.Object);

        var result = await handler.Handle(new SearchCitiesQuery(string.Empty, "moscow"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Country");
        citySearchService.Verify(
            service => service.SearchAsync(It.IsAny<string>(), It.IsAny<CitySearchTerm>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsServiceResult()
    {
        var cities = new[] { new CityDto(1, "Moscow", "Moscow (Russia)", 55.75, 37.62) };
        var expected = Result<IReadOnlyCollection<CityDto>>.Success(cities);

        var citySearchService = new Mock<ICitySearchService>();
        citySearchService
            .Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<CitySearchTerm>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new SearchCitiesQueryHandler(citySearchService.Object);

        var result = await handler.Handle(new SearchCitiesQuery("RU", "moscow"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Equal(cities);
    }
}