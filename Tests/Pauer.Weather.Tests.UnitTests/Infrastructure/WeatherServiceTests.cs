using FluentAssertions;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Infrastructure;

public sealed class WeatherServiceTests
{
    private readonly Mock<IWeatherService> _weatherService = new();
    private readonly ForecastDays _forecastDays = ForecastDays.Create(2).Value;

    [Fact]
    public async Task GetWeatherAsync_SuccessResult_ReturnsConfiguredResult()
    {
        var coordinates = Coordinates.Create(-56.77, -34.73).Value;
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        _weatherService
            .Setup(service => service.GetWeatherAsync(coordinates, _forecastDays, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var result = await _weatherService.Object.GetWeatherAsync(coordinates, _forecastDays, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected.Value);
    }

    [Fact]
    public async Task GetWeatherAsync_RespectsCancellationToken()
    {
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        _weatherService
            .Setup(service => service.GetWeatherAsync(It.IsAny<Coordinates>(), _forecastDays, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var coordinates = Coordinates.Create(0, 0).Value;
        using var cts = new CancellationTokenSource();

        await _weatherService.Object.GetWeatherAsync(coordinates, _forecastDays, cts.Token);

        cts.Token.IsCancellationRequested.Should().BeFalse();
    }
}
