namespace Pauer.Weather.Application.Cities.Dto;

public sealed record CityDto(int Id, string Name, string FullName, double Latitude, double Longitude);