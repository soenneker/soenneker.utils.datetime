using Soenneker.Extensions.DateTime;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Soenneker.Extensions.DateTime.Month;
using Soenneker.Extensions.DateTime.Week;

namespace Soenneker.Utils.DateTime;

/// <summary>
/// A utility library for helpful DateTime related operations
/// </summary>
public static class DateTimeUtil
{
    /// <summary>
    /// Builds a new <see cref="System.DateTime"/> instance representing a UTC date and time, 
    /// with optional year, month, day, hour, minute, and second parameters. If any of these parameters
    /// are not provided, the current UTC date and time values are used as defaults.
    /// </summary>
    /// <remarks>
    /// The current UTC date and time is used to fill in any parameters not provided. This approach ensures that the method
    /// is efficient by avoiding unnecessary null checks.
    /// </remarks>
    /// <param name="year">The year component of the date and time. Defaults to the current UTC year if null.</param>
    /// <param name="month">The month component of the date and time. Defaults to the current UTC month if null.</param>
    /// <param name="day">The day component of the date and time. Defaults to the current UTC day if null.</param>
    /// <param name="hour">The hour component of the date and time. Defaults to the current UTC hour if null.</param>
    /// <param name="minute">The minute component of the date and time. Defaults to the current UTC minute if null.</param>
    /// <param name="second">The second component of the date and time. Defaults to the current UTC second if null.</param>
    /// <returns>A <see cref="System.DateTime"/> object set to the specified date and time in UTC.</returns>
    [Pure]
    public static System.DateTime CreateUtcDateTime(int? year = null, int? month = null, int? day = null, int? hour = null, int? minute = null, int? second = null)
    {
        System.DateTime utcNow = System.DateTime.UtcNow; // Q: is this faster than checking for all nulls for these values?

        year ??= utcNow.Year;
        month ??= utcNow.Month;
        day ??= utcNow.Day;
        hour ??= utcNow.Hour;
        minute ??= utcNow.Minute;
        second ??= utcNow.Second;

        return new System.DateTime(year.Value, month.Value, day.Value, hour.Value, minute.Value, second.Value, DateTimeKind.Utc);
    }

    /// <summary>
    /// Builds a new <see cref="System.DateTime"/> instance representing a date and time 
    /// interpreted in a specific time zone, with optional year, month, day, hour, minute, and second parameters.
    /// If any parameter is not provided, the current value in that time zone is used as its default.
    /// </summary>
    /// <remarks>
    /// The supplied components represent wall-clock time in <paramref name="timeZoneInfo"/>. The returned value is the corresponding UTC instant.
    /// </remarks>
    /// <param name="timeZoneInfo">The <see cref="TimeZoneInfo"/> representing the target time zone for the date and time.</param>
    /// <param name="year">The year component, or the current year in the specified time zone.</param>
    /// <param name="month">The month component, or the current month in the specified time zone.</param>
    /// <param name="day">The day component, or the current day in the specified time zone.</param>
    /// <param name="hour">The hour component, or the current hour in the specified time zone.</param>
    /// <param name="minute">The minute component, or the current minute in the specified time zone.</param>
    /// <param name="second">The second component, or the current second in the specified time zone.</param>
    /// <returns>The UTC instant represented by the wall-clock components in <paramref name="timeZoneInfo"/>.</returns>
    [Pure]
    public static System.DateTime CreateTzDateTime(TimeZoneInfo timeZoneInfo, int? year = null, int? month = null, int? day = null, int? hour = null, int? minute = null, int? second = null)
    {
        System.DateTime now = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.UtcNow, timeZoneInfo);

        var wallTime = new System.DateTime(year ?? now.Year, month ?? now.Month, day ?? now.Day, hour ?? now.Hour, minute ?? now.Minute,
            second ?? now.Second, DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc(wallTime, timeZoneInfo);
    }

    /// <summary>
    /// Generates a list of weekly date ranges between the specified start and end dates, based on the given time zone.
    /// </summary>
    /// <param name="startAt">The start date and time.</param>
    /// <param name="endAt">The end date and time.</param>
    /// <param name="timeZoneInfo">The time zone to consider for week calculations.</param>
    /// <returns>
    /// A list of tuples, each containing a weekly start and end date within the specified range.
    /// </returns>
    /// <remarks>
    /// The method ensures that each week starts and ends according to the specified time zone's week start.
    /// </remarks>
    [Pure]
    public static List<(System.DateTime startAt, System.DateTime endAt)> GetWeeklyDateTimesBetween(System.DateTime startAt, System.DateTime endAt, TimeZoneInfo timeZoneInfo)
    {
        System.DateTime startDate = startAt.ToStartOfTzWeek(timeZoneInfo);
        System.DateTime endDate = startDate.ToEndOfTzWeek(timeZoneInfo);
        int capacity = endAt <= endDate ? 1 : 1 + (int)Math.Ceiling((endAt - endDate).TotalDays / 7d);
        var result = new List<(System.DateTime startAt, System.DateTime endAt)>(capacity);

        result.Add((startDate, endDate));

        while (endDate < endAt)
        {
            startDate = startDate.AddDays(7);
            endDate = endDate.AddDays(7);

            result.Add((startDate, endDate));
        }

        return result;
    }


    /// <summary>
    /// Generates a list of monthly date ranges between the specified start and end dates, based on the given time zone.
    /// </summary>
    /// <param name="startAt">The start date and time.</param>
    /// <param name="endAt">The end date and time.</param>
    /// <param name="timeZoneInfo">The time zone to consider for month calculations.</param>
    /// <returns>
    /// A list of tuples, each containing a monthly start and end date within the specified range.
    /// </returns>
    /// <remarks>
    /// The method ensures that each month starts and ends according to the specified time zone's month start.
    /// </remarks>
    [Pure]
    public static List<(System.DateTime startAt, System.DateTime endAt)> GetMonthlyDateTimesBetween(System.DateTime startAt, System.DateTime endAt, TimeZoneInfo timeZoneInfo)
    {
        System.DateTime startDate = startAt.ToStartOfTzMonth(timeZoneInfo);
        System.DateTime endDate = startDate.ToEndOfTzMonth(timeZoneInfo);
        int capacity = Math.Max(1, (endAt.Year - startDate.Year) * 12 + endAt.Month - startDate.Month + 1);
        var result = new List<(System.DateTime startAt, System.DateTime endAt)>(capacity);

        result.Add((startDate, endDate));

        while (endDate < endAt)
        {
            startDate = startDate.AddMonths(1);
            endDate = endDate.AddMonths(1);

            result.Add((startDate, endDate));
        }

        return result;
    }
}
