namespace Pauer.Weather.Application.Common;

public record struct Coordinates
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    
    public static Result<Coordinates> Create(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90)
            return Result<Coordinates>.Failure("Latitude must be between -90 and 90.");

        if (longitude is < -180 or > 180)
            return Result<Coordinates>.Failure("Longitude must be between -180 and 180.");

        return Result<Coordinates>.Success(new Coordinates
        {
            Latitude = latitude,
            Longitude = longitude
        });
    }
}