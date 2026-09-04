using Pauer.Weather.Domain.Results;

namespace Pauer.Weather.Domain.ValueObjects;

public readonly record struct ForecastDays
{
    private const int MinDays = 1;
    private const int MaxDays = 3;

    public int Value { get; }

    private ForecastDays(int value) => Value = value;

    public static Result<ForecastDays> Create(int days)
    {
        if (days is < MinDays or > MaxDays)
        {
            return Result<ForecastDays>.Failure($"Days must be between {MinDays} and {MaxDays}.");
        }

        return Result<ForecastDays>.Success(new ForecastDays(days));
    }
}