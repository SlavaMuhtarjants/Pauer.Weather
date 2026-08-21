using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Pauer.Weather.Application.Configuration;
using Pauer.Weather.Application.GetWeather;

namespace Pauer.Weather.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WeatherLocationSettings>(configuration.GetSection("WeatherLocation"));
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetWeatherQuery>());

        return services;
    }
}