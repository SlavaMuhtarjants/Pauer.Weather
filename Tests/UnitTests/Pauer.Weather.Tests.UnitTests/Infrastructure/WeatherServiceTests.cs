using FluentAssertions;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Common;
using Pauer.Weather.Application.GetWeather.Dto;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Infrastructure;

public sealed class WeatherServiceTests
{
    private readonly Mock<IWeatherService> _weatherService = new();

    [Fact]
    public async Task GetWeatherAsync_SuccessResult_ReturnsConfiguredResult()
    {
        var coordinates = Coordinates.Create(-56.77, -34.73).Value;
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        _weatherService
            .Setup(service => service.GetWeatherAsync(coordinates, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var result = await _weatherService.Object.GetWeatherAsync(coordinates, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected.Value);
    }

    [Fact]
    public async Task GetWeatherAsync_RespectsCancellationToken()
    {
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        _weatherService
            .Setup(service => service.GetWeatherAsync(It.IsAny<Coordinates>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var coordinates = Coordinates.Create(0, 0).Value;
        using var cts = new CancellationTokenSource();

        await _weatherService.Object.GetWeatherAsync(coordinates, cts.Token);

        cts.Token.IsCancellationRequested.Should().BeFalse();
    }
}
