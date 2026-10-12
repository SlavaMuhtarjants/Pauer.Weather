using FluentAssertions;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.GetWeather;
using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Application;

public sealed class GetWeatherQueryHandlerTests
{
    [Fact]
    public async Task Handle_InvalidLatitude_ReturnsError()
    {
        var weatherService = new Mock<IWeatherService>();
        var handler = new GetWeatherQueryHandler(weatherService.Object);
        var expected = Result<WeatherDto>.Failure("Latitude must be between -90 and 90.");

        var result = await handler.Handle(new GetWeatherQuery(100, 0), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expected.Error);
    }

    [Fact]
    public async Task Handle_ValidCoordinatesButInvalidDays_ReturnsError()
    {
        var coordinates = Coordinates.Create(-56.77, -34.73).Value;
        var expected = Result<WeatherDto>.Failure("Days must be between 1 and 3.");
        var query = new GetWeatherQuery(-56.77, -34.73, 5);

        var weatherService = new Mock<IWeatherService>();
        weatherService
            .Setup(service => service.GetWeatherAsync(
                coordinates,
                It.IsAny<ForecastDays>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetWeatherQueryHandler(weatherService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expected.Error);
    }

    [Fact]
    public async Task Handle_ValidCoordinatesAndValidDays_ReturnsOk()
    {
        var coordinates = Coordinates.Create(-56.77, -34.73).Value;
        var forecastDays = ForecastDays.Create(1).Value;
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        var query = new GetWeatherQuery(-56.77, -34.73, 1);

        var weatherService = new Mock<IWeatherService>();
        weatherService
            .Setup(service => service.GetWeatherAsync(coordinates, forecastDays, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var handler = new GetWeatherQueryHandler(weatherService.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected.Value);
    }
}