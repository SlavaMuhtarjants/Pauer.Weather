using FluentAssertions;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Countries;
using Pauer.Weather.Application.Countries.Dto;
using Pauer.Weather.Domain.Results;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Application;

public sealed class GetCountriesQueryHandlerTests
{
    [Fact]
    public async Task Handle_RepositoryReturnsSuccess_ReturnsOk()
    {
        var countries = new[] { new CountryDto("RU", "Russia"), new CountryDto("GB", "United Kingdom") };
        var expected = Result<IReadOnlyCollection<CountryDto>>.Success(countries);

        var repository = new Mock<ICountryRepository>();
        repository
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetCountriesQueryHandler(repository.Object);

        var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Equal(countries);
    }

    [Fact]
    public async Task Handle_RepositoryReturnsFailure_PropagatesError()
    {
        var expected = Result<IReadOnlyCollection<CountryDto>>.Failure("Database error");

        var repository = new Mock<ICountryRepository>();
        repository
            .Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetCountriesQueryHandler(repository.Object);

        var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Database error");
    }
}