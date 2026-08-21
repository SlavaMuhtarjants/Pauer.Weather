using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Pauer.Weather.Application;
using Pauer.Weather.Infrastructure.Configuration;

namespace Pauer.Weather.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WeatherApiSettings>(configuration.GetSection("WeatherApi"));

        services.AddHttpClient<WeatherApiHttpClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WeatherApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddScoped<IWeatherService, WeatherService>();

        return services;
    }
}