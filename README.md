# Pauer.Weather

A Blazor Server application that displays a weather forecast for a location selected by the user. Countries are loaded from a PostgreSQL database, and towns and cities are searched through GeoAPI. Forecasts are powered by WeatherAPI.com.

## Tech Stack

- **.NET 10** / ASP.NET
- **Blazor Server** for the web UI
- **MediatR** (v14.2.0)
- **Entity Framework Core** with **PostgreSQL** (Npgsql) for persistence
- **Serilog** for structured logging
- Testing: xUnit + FluentAssertions

## Weather API usage
http://api.weatherapi.com/v1/current.json has not been utilised because its response is already included in the response from the http://api.weatherapi.com/v1/forecast.json endpoint.

## Architecture

The solution follows the Clean Architecture with a clear separation of concerns:

### Pauer.Weather.UI
The Blazor Server web application and composition root. Configures dependency injection, registers all application services, sets up Serilog logging. The location picker (country and city search) lives here.

### Pauer.Weather.Application
Contains the business logic layer with MediatR queries and handlers: weather forecast, city search and country list.

### Pauer.Weather.Infrastructure
Handles integration with the external APIs and persistence:
- WeatherAPI.com for forecasts
- GeoAPI for city search (cached in memory for 10 minutes)
- PostgreSQL via EF Core for the country list (`WeatherForecastDbContext`, migrations)

### Pauer.Weather.Domain
Contains core domain models (such as the `Country` entity), value objects (such as `Coordinates`, `ForecastDays` and `CitySearchTerm`), and common result abstractions.

### Architecture Rule
The Application layer defines interfaces and the business logic in handlers; Infrastructure implements interaction with the external APIs and the database. The UI communicates with Application through MediatR only.

## Configuration

Connection string and API settings are configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "WeatherForecast": "Host=localhost;Port=5432;Database=weather_forecast;Username=postgres;Password=postgres"
  },
  "WeatherApi": {
    "BaseUrl": "http://api.weatherapi.com/v1/",
    "ApiKey": "your-api-key-here"
  },
  "GeoApi": {
    "BaseUrl": "https://htmlweb.ru/",
    "ApiKey": "your-api-key-here"
  }
}
```

The forecast location is no longer configured in the file. The user selects a country and searches for a town or city in the UI. City search requires at least 4 characters.

## Running the Application

A PostgreSQL database is required. Apply the migrations before the first run:

```bash
dotnet user-secrets init --project Pauer.Weather.UI
dotnet user-secrets set "WeatherApi:ApiKey" "YOUR_API_KEY" --project Pauer.Weather.UI
dotnet user-secrets set "GeoApi:ApiKey" "YOUR_GEO_API_KEY" --project Pauer.Weather.UI
dotnet user-secrets set "ConnectionStrings:WeatherForecast" "YOUR_CONNECTION_STRING" --project Pauer.Weather.UI
dotnet ef database update --project Pauer.Weather.Infrastructure --startup-project Pauer.Weather.UI
dotnet run --project Pauer.Weather.UI 
```

The application will start on the configured Blazor Server port. Select a country, type at least 4 characters of a town or city name, and pick a result to display its weather forecast.

## Testing

A unit test project is located in `Pauer.Weather.Tests.UnitTests/` and uses xUnit and FluentAssertions. Test naming follows the convention: `[Method]_[Scenario]_[ExpectedResult]`. Tests cover the Application layer (handlers) and Domain value objects.

```bash
dotnet test Pauer.Weather.slnx 
```
