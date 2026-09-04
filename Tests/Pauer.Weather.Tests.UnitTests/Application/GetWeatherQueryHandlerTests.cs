using FluentAssertions;

using Microsoft.Extensions.Options;

using Moq;

using Pauer.Weather.Application;
using Pauer.Weather.Application.Configuration;
using Pauer.Weather.Application.GetWeather;
using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Domain.Results;
using Pauer.Weather.Domain.ValueObjects;

using Xunit;

namespace Pauer.Weather.Tests.UnitTests.Application;

public sealed class GetWeatherQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_InvalidCoordinates_ReturnsError()
    {
        var weatherService = new Mock<IWeatherService>();
        var locationOptions = Options.Create(new WeatherLocationSettings 
        { 
            Latitude = 100
        });
        
        var handler = new GetWeatherQueryHandler(weatherService.Object, locationOptions);
        var expected = Result<WeatherDto>.Failure("Latitude must be between -90 and 90.");

        var result = await handler.Handle(new GetWeatherQuery(), CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expected.Error);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCoordinatesButInvalidDays_ReturnsError()
    {
        var locationOptions = Options.Create(new WeatherLocationSettings 
        { 
            Latitude = -56.77, 
            Longitude = -34.73 
        });
        var coordinatesResult = Coordinates.Create(locationOptions.Value.Latitude, locationOptions.Value.Longitude);
        var expected = Result<WeatherDto>.Failure("Days must be between 1 and 3.");
        var query = new GetWeatherQuery();
        
        var weatherService = new Mock<IWeatherService>();
        weatherService
            .Setup(service => service.GetWeatherAsync(
                coordinatesResult.Value,
                ForecastDays.Create(query.Days).Value,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var handler = new GetWeatherQueryHandler(weatherService.Object, locationOptions);

        var result = await handler.Handle(query, CancellationToken.None);
        
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expected.Error);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCoordinatesAndValidDays_ReturnsOk()
    {
        var locationOptions = Options.Create(new WeatherLocationSettings 
        { 
            Latitude = -56.77, 
            Longitude = -34.73 
        });
        var coordinatesResult = Coordinates.Create(locationOptions.Value.Latitude, locationOptions.Value.Longitude);
        var expected = Result<WeatherDto>.Success(It.IsAny<WeatherDto>());
        var query = new GetWeatherQuery(1);
        
        var weatherService = new Mock<IWeatherService>();
        weatherService
            .Setup(service => service.GetWeatherAsync(
                coordinatesResult.Value,
                ForecastDays.Create(query.Days).Value,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var handler = new GetWeatherQueryHandler(weatherService.Object, locationOptions);

        var result = await handler.Handle(query, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected.Value);
    }
}