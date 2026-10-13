using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Pauer.Weather.Application;
using Pauer.Weather.Infrastructure.Configuration;
using Pauer.Weather.Infrastructure.Persistence;

namespace Pauer.Weather.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WeatherApiSettings>(configuration.GetSection("WeatherApi"));
        services.Configure<GeoApiSettings>(configuration.GetSection("GeoApi"));

        services.AddDbContext<WeatherForecastDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("WeatherForecast"))
            .UseSnakeCaseNamingConvention());

        services.AddMemoryCache();

        services.AddHttpClient<WeatherApiHttpClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WeatherApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddHttpClient<GeoApiHttpClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<GeoApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddScoped<IWeatherService, WeatherService>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICitySearchService, CitySearchService>();

        return services;
    }
}