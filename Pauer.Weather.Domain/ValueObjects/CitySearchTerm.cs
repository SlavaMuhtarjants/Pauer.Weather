using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Domain.ValueObjects;

public readonly record struct CitySearchTerm
{
    private const int MinLength = 4;

    public string Value { get; }

    private CitySearchTerm(string value) => Value = value;

    public static Result<CitySearchTerm> Create(string? term)
    {
        var trimmed = term?.Trim() ?? string.Empty;

        if (trimmed.Length < MinLength)
        {
            return Result<CitySearchTerm>.Failure($"Search term must be longer than {MinLength - 1} characters.");
        }

        return Result<CitySearchTerm>.Success(new CitySearchTerm(trimmed));
    }
}