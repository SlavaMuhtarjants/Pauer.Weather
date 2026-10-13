## Overview
This solution provides a weather forecast for the location defined in the configuration file.

## Tech Stack
- .NET 10, ASP.NET
- MediatR for CQRS (https://github.com/LuckyPennySoftware/MediatR)
- Blazor Server
- xUnit + FluentAssertions for testing

## Application Structure
- `Pauer.Weather.Domain/` - Domain models, value objects, and business abstractions
- `Pauer.Weather.Application/` - Requests, handlers, validators, business logic implementation
- `Pauer.Weather.Infrastructure/` - Integration with Weather API, mapping to business logic DTOs
- `Pauer.Weather.UI/` - Blazor app, middleware, DI configuration, user interface
- `Pauer.Weather.Tests.UnitTests/` - Application layer tests

## Architecture Rules
- Application layer defines interfaces, Infrastructure implements them
- Application layer must not reference Infrastructure
- Use MediatR for application use cases. Blazor components must communicate through MediatR, not directly with Infrastructure.

## Code Conventions
- Positional records for request/response DTOs
- File-scoped namespaces
- Primary constructors for dependency injection
- Sealed classes for implementations
- Internal by default, public only for contracts

### Patterns We Use
- Primary constructors for DI
- Records for DTOs
- Perfer sealed classes
- Result<T> pattern for error handling (no exceptions for flow control)
- Always pass CancellationToken to async methods
- 
## Patterns We Do NOT Use
- AutoMapper or any mapping library

## Testing
- Unit tests: Application logic and handlers
- Use FluentAssertions for readable assertions
- Test naming: `[Method]_[Scenario]_[ExpectedResult]`