using Pauer.Weather.Application.GetWeather.Dto;
using Pauer.Weather.Infrastructure.Contracts;

using System.Globalization;

namespace Pauer.Weather.Infrastructure.Mapping;

internal static class WeatherApiMapper
{
    private const string LocalTimeFormat = "yyyy-MM-dd HH:mm";
    private const string HourTimeFormat = "yyyy-MM-dd HH:mm";
    private const string DateFormat = "yyyy-MM-dd";

    public static WeatherDto ToWeatherDto(WeatherApiForecastResponse forecast)
    {
        var localNow = DateTime.ParseExact(forecast.Location.LocalTime, LocalTimeFormat, CultureInfo.InvariantCulture);

        var currentWeather = new CurrentWeatherDto(
            forecast.Location.Name,
            localNow,
            forecast.Current.TempC,
            forecast.Current.FeelsLikeC,
            forecast.Current.Humidity,
            forecast.Current.WindKph,
            forecast.Current.Condition.Text,
            ToAbsoluteIconUrl(forecast.Current.Condition.Icon));

        var forecastDays = forecast.Forecast.ForecastDay;
        var todayDay = forecastDays.Count == 0 ? null : forecastDays.ElementAt(0);
        var tomorrowDay = forecastDays.Count < 2 ? null : forecastDays.ElementAt(1);

        var today = todayDay == null
            ? null
            : new DayForecastDto(
                ParseDate(todayDay.Date),
                todayDay.Hour
                    .Where(hour =>
                        ParseHourTime(hour.Time) >= new DateTime(localNow.Year, localNow.Month, localNow.Day,
                            localNow.Hour, 0, 0))
                    .Select(ToHourlyForecastDto)
                    .ToArray());

        var tomorrow = tomorrowDay == null
            ? null
            : new DayForecastDto(
                ParseDate(tomorrowDay.Date),
                tomorrowDay.Hour.Select(ToHourlyForecastDto).ToArray());

        var allDays = forecastDays
            .Select((day, index) => new DaySummaryDto(
                ParseDate(day.Date),
                ToDayLabel(index, ParseDate(day.Date)),
                day.Day.MinTempC,
                day.Day.MaxTempC,
                ToAbsoluteIconUrl(day.Day.Condition.Icon)))
            .ToArray();

        return new WeatherDto(currentWeather, new ForecastDto(today, tomorrow, allDays));
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