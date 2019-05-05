using System;

namespace Ical.Net.DataTypes
{
    [Obsolete]
    public interface IDateTime : IEncodableDataType, IComparable<CalDateTime>, IFormattable, ICalendarDataType
    {
        /// <summary>
        /// Converts the date/time to this computer's local date/time.
        /// </summary>
        DateTime AsSystemLocal { get; }

        /// <summary>
        /// Converts the date/time to UTC (Coordinated Universal Time)
        /// </summary>
        DateTime AsUtc { get; }

        /// <summary>
        /// Returns a DateTimeOffset representation of the Value. If a TzId is specified, it will use that time zone's UTC offset, otherwise it will use the
        /// system-local time zone.
        /// </summary>
        DateTimeOffset AsDateTimeOffset { get; }

        /// <summary>
        /// Gets/sets whether the Value of this date/time represents
        /// a universal time.
        /// </summary>
        bool IsUtc { get; }

        /// <summary>
        /// Gets the time zone name this time is in, if it references a time zone.
        /// </summary>
        string TimeZoneName { get; }

        /// <summary>
        /// Gets/sets the underlying DateTime value stored.  This should always
        /// use DateTimeKind.Utc, regardless of its actual representation.
        /// Use IsUtc along with the TZID to control how this
        /// date/time is handled.
        /// </summary>
        DateTime Value { get; set; }

        /// <summary>
        /// Gets/sets whether or not this date/time value contains a 'date' part.
        /// </summary>
        bool HasDate { get; set; }

        /// <summary>
        /// Gets/sets whether or not this date/time value contains a 'time' part.
        /// </summary>
        bool HasTime { get; set; }

        /// <summary>
        /// Gets/sets the time zone ID for this date/time value.
        /// </summary>
        string TzId { get; set; }

        /// <summary>
        /// Gets the year for this date/time value.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Gets the month for this date/time value.
        /// </summary>
        int Month { get; }

        /// <summary>
        /// Gets the day for this date/time value.
        /// </summary>
        int Day { get; }

        /// <summary>
        /// Gets the hour for this date/time value.
        /// </summary>
        int Hour { get; }

        /// <summary>
        /// Gets the minute for this date/time value.
        /// </summary>
        int Minute { get; }

        /// <summary>
        /// Gets the second for this date/time value.
        /// </summary>
        int Second { get; }

        /// <summary>
        /// Gets the millisecond for this date/time value.
        /// </summary>
        int Millisecond { get; }

        /// <summary>
        /// Gets the ticks for this date/time value.
        /// </summary>
        long Ticks { get; }

        /// <summary>
        /// Gets the DayOfWeek for this date/time value.
        /// </summary>
        DayOfWeek DayOfWeek { get; }

        /// <summary>
        /// Gets the date portion of the date/time value.
        /// </summary>
        DateTime Date { get; }

        /// <summary>
        /// Converts the date/time value to a local time
        /// within the specified time zone.
        /// </summary>
        CalDateTime ToTimeZone(string tzId);

        CalDateTime Add(TimeSpan ts);
        CalDateTime Subtract(TimeSpan ts);
        TimeSpan Subtract(CalDateTime dt);

        CalDateTime AddYears(int years);
        CalDateTime AddMonths(int months);
        CalDateTime AddDays(int days);
        CalDateTime AddHours(int hours);
        CalDateTime AddMinutes(int minutes);
        CalDateTime AddSeconds(int seconds);
        CalDateTime AddMilliseconds(int milliseconds);
        CalDateTime AddTicks(long ticks);

        bool LessThan(CalDateTime dt);
        bool GreaterThan(CalDateTime dt);
        bool LessThanOrEqual(CalDateTime dt);
        bool GreaterThanOrEqual(CalDateTime dt);

        void AssociateWith(CalDateTime dt);
    }
}