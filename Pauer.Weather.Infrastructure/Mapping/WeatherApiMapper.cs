using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Infrastructure.Contracts;

using System.Globalization;

namespace Pauer.Weather.Infrastructure.Mapping;

internal static class WeatherApiMapper
{
    private const string LocalTimeFormat = "yyyy-MM-dd HH:mm";
    private const string HourTimeFormat = "yyyy-MM-dd HH:mm";
    private const string DateFormat = "yyyy-MM-dd";

    public static WeatherDto ToWeatherDto(WeatherApiCurrentResponse current, WeatherApiForecastResponse forecast)
    {
        var localNow = DateTime.ParseExact(current.Location.LocalTime, LocalTimeFormat, CultureInfo.InvariantCulture);

        var currentWeather = new CurrentWeatherDto(
            current.Location.Name,
            localNow,
            current.Current.TempC,
            current.Current.FeelsLikeC,
            current.Current.Humidity,
            current.Current.WindKph,
            current.Current.Condition.Text,
            ToAbsoluteIconUrl(current.Current.Condition.Icon));

        var forecastDays = forecast.Forecast.ForecastDay;
        var todayDay = forecastDays.Count == 0 ? null : forecastDays[0];
        var tomorrowDay = forecastDays.Count < 2 ? null : forecastDays[1];

        var today = todayDay == null
            ? null
            : new DayForecastDto(
                ParseDate(todayDay.Date),
                todayDay.Hour
                    .Where(hour =>
                        ParseHourTime(hour.Time) >= new DateTime(localNow.Year, localNow.Month, localNow.Day,
                            localNow.Hour, 0, 0))
                    .Select(ToHourlyForecastDto)
                    .ToList());

        var tomorrow = tomorrowDay == null
            ? null
            : new DayForecastDto(
                ParseDate(tomorrowDay.Date),
                tomorrowDay.Hour.Select(ToHourlyForecastDto).ToList());

        var threeDay = forecastDays
            .Take(3)
            .Select((day, index) => new DaySummaryDto(
                ParseDate(day.Date),
                ToDayLabel(index, ParseDate(day.Date)),
                day.Day.MinTempC,
                day.Day.MaxTempC,
                ToAbsoluteIconUrl(day.Day.Condition.Icon)))
            .ToList();

        return new WeatherDto(currentWeather, new ForecastDto(today, tomorrow, threeDay));
    }

    private static HourlyForecastDto ToHourlyForecastDto(WeatherApiHourResponse hour) =>
        new(ParseHourTime(hour.Time), hour.TempC, ToAbsoluteIconUrl(hour.Condition.Icon));

    private static string ToDayLabel(int index, DateOnly date) => index switch
    {
        0 => "Today",
        1 => "Tomorrow",
        _ => date.DayOfWeek.ToString()[..3],
    };

    private static DateOnly ParseDate(string date) =>
        DateOnly.ParseExact(date, DateFormat, CultureInfo.InvariantCulture);

    private static DateTime ParseHourTime(string time) =>
        DateTime.ParseExact(time, HourTimeFormat, CultureInfo.InvariantCulture);

    private static string ToAbsoluteIconUrl(string icon) =>
        icon.StartsWith("//", StringComparison.Ordinal) ? $"https:{icon}" : icon;
}