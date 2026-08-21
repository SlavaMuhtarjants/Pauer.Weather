# Pauer.Weather

A Blazor Server application that displays a weather forecast for a location defined in the application configuration, powered by WeatherAPI.com.

## Tech Stack

- **.NET 10** / ASP.NET
- **Blazor Server** for the web UI
- **MediatR** (v14.2.0)
- **Serilog** for structured logging
- Testing: xUnit + FluentAssertions (planned)

## Architecture

The solution follows a layered architecture with a clear separation of concerns:

### Pauer.Weather.UI
The Blazor Server web application and composition root. Configures dependency injection, registers all application services, sets up Serilog logging.

### Pauer.Weather.Application
Contains the business logic layer with MediatR queries and handlers.

### Pauer.Weather.Infrastructure
Handles integration with WeatherAPI.com.

### Pauer.Weather.Domain
Absent as there are no domain types and entities.

### Architecture Rule
The Application layer defines interfaces; Infrastructure implements them. The UI communicates with Application through MediatR only.

## Configuration

Location and weather API settings are configured in `appsettings.json`:

```json
{
  "WeatherLocation": {
    "Latitude": 55.7558,
    "Longitude": 37.6173
  },
  "WeatherApi": {
    "BaseUrl": "http://api.weatherapi.com/v1/",
    "ApiKey": "your-api-key-here"
  }
}
```

## Running the Application

```bash
dotnet run --project src/Pauer.Weather.UI
```

The application will start on the configured Blazor Server port and display the weather forecast for the configured location.

## Testing

A unit test project will be added under `tests/UnitTests/` using xUnit and FluentAssertions. Test naming follows the convention: `[Method]_[Scenario]_[ExpectedResult]`. Tests will cover the Application layer (handlers, validators) and critical Infrastructure logic.
