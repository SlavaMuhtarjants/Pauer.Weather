namespace Pauer.Weather.Application.GetWeather.Dto;

public sealed record DaySummaryDto(
    DateOnly Date,
    string Label,
    double MinTemp,
    double MaxTemp,
    string ConditionIconUrl);