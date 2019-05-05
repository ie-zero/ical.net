using System;
using System.IO;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;
using Ical.Net.Utility;
using NodaTime;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// The iCalendar equivalent of the .NET <see cref="DateTime"/> class.
    /// <remarks>
    /// In addition to the features of the <see cref="DateTime"/> class, the <see cref="CalDateTime"/>
    /// class handles time zone differences, and integrates seamlessly into the iCalendar framework.
    /// </remarks>
    /// </summary>
    public sealed class CalDateTime : EncodableDataType, IComparable<CalDateTime>
    {
        public static CalDateTime Now => new CalDateTime(DateTime.Now);

        public static CalDateTime Today => new CalDateTime(DateTime.Today);

        private string _tzId = string.Empty;
        private DateTime _asUtc = DateTime.MinValue;
        private DateTime _value;

        public CalDateTime() { }

        public CalDateTime(CalDateTime value)
        {
            Initialize(value.Value, value.TzId, null);
        }

        public CalDateTime(DateTime value) : this(value, null) { }

        /// <summary>
        /// Specifying a `tzId` value will override `value`'s `DateTimeKind` property. If the time zone specified is UTC, the underlying `DateTimeKind` will be
        /// `Utc`. If a non-UTC time zone is specified, the underlying `DateTimeKind` property will be `Local`. If no time zone is specified, the `DateTimeKind`
        /// property will be left untouched.
        /// </summary>
        public CalDateTime(DateTime value, string tzId)
        {
            Initialize(value, tzId, null);
        }

        public CalDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            Initialize(year, month, day, hour, minute, second, null, null);
            HasTime = true;
        }

        public CalDateTime(int year, int month, int day, int hour, int minute, int second, string tzId)
        {
            Initialize(year, month, day, hour, minute, second, tzId, null);
            HasTime = true;
        }

        public CalDateTime(int year, int month, int day, int hour, int minute, int second, string tzId, Calendar cal)
        {
            Initialize(year, month, day, hour, minute, second, tzId, cal);
            HasTime = true;
        }

        public CalDateTime(int year, int month, int day) : this(year, month, day, 0, 0, 0) { }

        public CalDateTime(int year, int month, int day, string tzId) : this(year, month, day, 0, 0, 0, tzId) { }

        public CalDateTime(string value)
        {
            var serializer = new DateTimeSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }
        
        /// <summary>
        /// Converts the date/time to the date/time of the computer running the program. If the DateTimeKind is Unspecified, it's assumed that the underlying
        /// Value already represents the system's datetime.
        /// </summary>
        public DateTime AsSystemLocal
        {
            get
            {
                if (Value.Kind == DateTimeKind.Unspecified)
                {
                    return HasTime
                        ? Value
                        : Value.Date;
                }

                return HasTime
                    ? Value.ToLocalTime()
                    : Value.ToLocalTime().Date;
            }
        }

        /// <summary>
        /// Returns a representation of the DateTime in Coordinated Universal Time (UTC)
        /// </summary>
        public DateTime AsUtc
        {
            get
            {
                if (_asUtc == DateTime.MinValue)
                {
                    // In order of weighting:
                    //  1) Specified TzId
                    //  2) Value having a DateTimeKind.Utc
                    //  3) Use the OS's time zone

                    if (!string.IsNullOrWhiteSpace(TzId))
                    {
                        var asLocal = DateUtil.ToZonedDateTimeLeniently(Value, TzId);
                        _asUtc = asLocal.ToDateTimeUtc();
                    }
                    else if(IsUtc || Value.Kind == DateTimeKind.Utc)
                    {
                        _asUtc = DateTime.SpecifyKind(Value, DateTimeKind.Utc);
                    }
                    else
                    {
                        _asUtc = DateTime.SpecifyKind(Value, DateTimeKind.Local).ToUniversalTime();
                    }
                }
                return _asUtc;
            }
        }

        /// <summary>
        /// Gets/sets the underlying DateTime value stored.  This should always
        /// use DateTimeKind.Utc, regardless of its actual representation.
        /// Use IsUtc along with the TZID to control how this
        /// date/time is handled.
        /// </summary>
        public DateTime Value
        {
            get => _value;
            set
            {
                if (_value == value && _value.Kind == value.Kind)
                {
                    return;
                }

                _asUtc = DateTime.MinValue;
                _value = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the Value of this date/time represents a universal time.
        /// </summary>
        public bool IsUtc => _value.Kind == DateTimeKind.Utc;

        /// <summary>
        /// Gets/sets whether or not this date/time value contains a 'date' part.
        /// </summary>
        public bool HasDate { get; set; }

        /// <summary>
        /// Gets/sets whether or not this date/time value contains a 'time' part.
        /// </summary>
        public bool HasTime { get; set; }

        /// <summary>
        /// Setting the TzId to a local time zone will set Value.Kind to Local. Setting TzId to UTC will set Value.Kind to Utc. If the incoming value is null
        /// or whitespace, Value.Kind will be set to Unspecified. Setting the TzId will NOT incur a UTC offset conversion under any circumstances. To convert
        /// to another time zone, use the ToTimeZone() method.
        /// </summary>
        public string TzId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_tzId))
                {
                    _tzId = Parameters.Get("TZID");
                }
                return _tzId;
            }
            set
            {
                if (string.Equals(_tzId, value, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _asUtc = DateTime.MinValue;

                var isEmpty = string.IsNullOrWhiteSpace(value);
                if (isEmpty)
                {
                    Parameters.Remove("TZID");
                    _tzId = null;
                    Value = DateTime.SpecifyKind(Value, DateTimeKind.Local);
                    return;
                }

                var kind = string.Equals(value, "UTC", StringComparison.OrdinalIgnoreCase)
                    ? DateTimeKind.Utc
                    : DateTimeKind.Local;

                Value = DateTime.SpecifyKind(Value, kind);
                Parameters.Set("TZID", value);
                _tzId = value;
            }
        }

        /// <summary>
        /// Gets the time zone name this time is in, if it references a time zone.
        /// </summary>
        public string TimeZoneName => TzId;

        public int Year => Value.Year;

        public int Month => Value.Month;

        public int Day => Value.Day;

        public int Hour => Value.Hour;

        public int Minute => Value.Minute;

        public int Second => Value.Second;

        public int Millisecond => Value.Millisecond;

        public long Ticks => Value.Ticks;

        public DayOfWeek DayOfWeek => Value.DayOfWeek;

        public int DayOfYear => Value.DayOfYear;

        public DateTime Date => Value.Date;

        public TimeSpan TimeOfDay => Value.TimeOfDay;

        /// <summary>
        /// Returns a DateTimeOffset representation of the Value. If a TzId is specified, it will use that time zone's UTC offset, otherwise it will use the
        /// system-local time zone.
        /// </summary>
        public DateTimeOffset AsDateTimeOffset
        {
            get
            {
                return string.IsNullOrWhiteSpace(TzId)
                    ? new DateTimeOffset(AsSystemLocal)
                    : DateUtil.ToZonedDateTimeLeniently(Value, TzId).ToDateTimeOffset();
            }
        }

        private void Initialize(int year, int month, int day, int hour, int minute, int second, string tzId, Calendar cal)
        {
            Initialize(CoerceDateTime(year, month, day, hour, minute, second, DateTimeKind.Local), tzId, cal);
        }

        private void Initialize(DateTime value, string tzId, Calendar cal)
        {
            if (!string.IsNullOrWhiteSpace(tzId) && !tzId.Equals("UTC", StringComparison.OrdinalIgnoreCase))
            {
                // Definitely local
                value = DateTime.SpecifyKind(value, DateTimeKind.Local);
                TzId = tzId;
            }
            else if (string.Equals("UTC", tzId, StringComparison.OrdinalIgnoreCase) || value.Kind == DateTimeKind.Utc)
            {
                // Probably UTC
                value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                TzId = "UTC";
            }

            Value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Kind);
            HasDate = true;
            HasTime = value.Second != 0 || value.Minute != 0 || value.Hour != 0;
            AssociatedObject = cal;
        }

        private DateTime CoerceDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
        {
            var dt = DateTime.MinValue;

            // NOTE: determine if a date/time value exceeds the representable date/time values in .NET.
            // If so, let's automatically adjust the date/time to compensate.
            // FIXME: should we have a parsing setting that will throw an exception
            // instead of automatically adjusting the date/time value to the
            // closest representable date/time?
            try
            {
                if (year > 9999)
                {
                    dt = DateTime.MaxValue;
                }
                else if (year > 0)
                {
                    dt = new DateTime(year, month, day, hour, minute, second, kind);
                }
            }
            // TODO: Review code - exceptions are swallowed silently
            catch { }

            return dt;
        }

        public void AssociateWith(CalDateTime dt)
        {
            if (AssociatedObject == null && dt.AssociatedObject != null)
            {
                AssociatedObject = dt.AssociatedObject;
            }
            else if (AssociatedObject != null && dt.AssociatedObject == null)
            {
                dt.AssociatedObject = AssociatedObject;
            }
        }

        /// <summary>
        /// Returns a representation of the CalDateTime in the specified time zone
        /// </summary>
        public CalDateTime ToTimeZone(string tzId)
        {
            if (string.IsNullOrWhiteSpace(tzId))
            {
                throw new ArgumentException("You must provide a valid time zone id", nameof(tzId));
            }

            // If TzId is empty, it's a system-local datetime, so we should use the system time zone as the starting point.
            var originalTzId = string.IsNullOrWhiteSpace(TzId)
                ? TimeZoneInfo.Local.Id
                : TzId;

            var zonedOriginal = DateUtil.ToZonedDateTimeLeniently(Value, originalTzId);
            var converted = zonedOriginal.WithZone(DateUtil.GetZone(tzId));

            return converted.Zone == DateTimeZone.Utc
                ? new CalDateTime(converted.ToDateTimeUtc(), tzId)
                : new CalDateTime(DateTime.SpecifyKind(converted.ToDateTimeUnspecified(), DateTimeKind.Local), tzId);
        }

        public CalDateTime Add(TimeSpan ts)
        {
            return this + ts;
        }

        public CalDateTime Subtract(TimeSpan ts)
        {
            return this - ts;
        }

        public TimeSpan Subtract(CalDateTime dt)
        {
            return this - dt;
        }

        public CalDateTime AddYears(int years)
        {
            var dt = Copy<CalDateTime>();
            dt.Value = Value.AddYears(years);
            return dt;
        }

        public CalDateTime AddMonths(int months)
        {
            var dt = Copy<CalDateTime>();
            dt.Value = Value.AddMonths(months);
            return dt;
        }

        public CalDateTime AddDays(int days)
        {
            var dt = Copy<CalDateTime>();
            dt.Value = Value.AddDays(days);
            return dt;
        }

        public CalDateTime AddHours(int hours)
        {
            var dt = Copy<CalDateTime>();
            if (!dt.HasTime && hours % 24 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddHours(hours);
            return dt;
        }

        public CalDateTime AddMinutes(int minutes)
        {
            // TODO: Remove magic number.

            var dt = Copy<CalDateTime>();
            if (!dt.HasTime && minutes % 1440 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddMinutes(minutes);
            return dt;
        }

        public CalDateTime AddSeconds(int seconds)
        {
            // TODO: Remove magic number.

            var dt = Copy<CalDateTime>();
            if (!dt.HasTime && seconds % 86400 > 0)
            {
                dt.HasTime = true;
            }

            dt.Value = Value.AddSeconds(seconds);
            return dt;
        }

        public CalDateTime AddMilliseconds(int milliseconds)
        {
            // TODO: Remove magic number.

            var dt = Copy<CalDateTime>();
            if (!dt.HasTime && milliseconds % 86400000 > 0)
            {
                dt.HasTime = true;
            }

            dt.Value = Value.AddMilliseconds(milliseconds);
            return dt;
        }

        public CalDateTime AddTicks(long ticks)
        {
            var dt = Copy<CalDateTime>();
            dt.HasTime = true;
            dt.Value = Value.AddTicks(ticks);
            return dt;
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var dt = obj as CalDateTime;
            if (dt == null)
            {
                return;
            }

            _value = dt.Value;
            HasDate = dt.HasDate;
            HasTime = dt.HasTime;

            AssociateWith(dt);
        }

        public bool LessThan(CalDateTime dt)
        {
            return this < dt;
        }

        public bool GreaterThan(CalDateTime dt)
        {
            return this > dt;
        }

        public bool LessThanOrEqual(CalDateTime dt)
        {
            return this <= dt;
        }

        public bool GreaterThanOrEqual(CalDateTime dt)
        {
            return this >= dt;
        }

        public static bool operator <(CalDateTime left, CalDateTime right)
        {
            return left != null && right != null && left.AsUtc < right.AsUtc;
        }

        public static bool operator >(CalDateTime left, CalDateTime right)
        {
            return left != null && right != null && left.AsUtc > right.AsUtc;
        }

        public static bool operator <=(CalDateTime left, CalDateTime right)
        {
            return left != null && right != null && left.AsUtc <= right.AsUtc;
        }

        public static bool operator >=(CalDateTime left, CalDateTime right)
        {
            return left != null && right != null && left.AsUtc >= right.AsUtc;
        }

        public static bool operator ==(CalDateTime left, CalDateTime right)
        {
            return ReferenceEquals(left, null) || ReferenceEquals(right, null)
                ? ReferenceEquals(left, right)
                : right is CalDateTime
                    && left.Value.Equals(right.Value)
                    && left.HasDate == right.HasDate
                    && left.AsUtc.Equals(right.AsUtc)
                    && string.Equals(left.TzId, right.TzId, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(CalDateTime left, CalDateTime right)
        {
            return !(left == right);
        }

        public static TimeSpan operator -(CalDateTime left, CalDateTime right)
        {
            left.AssociateWith(right);
            return left.AsUtc - right.AsUtc;
        }

        public static CalDateTime operator -(CalDateTime left, TimeSpan right)
        {
            var copy = left.Copy<CalDateTime>();
            copy.Value -= right;
            return copy;
        }

        public static CalDateTime operator +(CalDateTime left, TimeSpan right)
        {
            var copy = left.Copy<CalDateTime>();
            copy.Value += right;
            return copy;
        }

        public static implicit operator CalDateTime(DateTime left) => new CalDateTime(left);

        public int CompareTo(CalDateTime dt)
        {
            if (Equals(dt))
            {
                return 0;
            }
            if (this < dt)
            {
                return -1;
            }
            if (this > dt)
            {
                return 1;
            }
            throw new Exception("An error occurred while comparing two CalDateTime values.");
        }

        public bool Equals(CalDateTime other)
            => this == other;

        public override bool Equals(object other)
            => other is CalDateTime && (CalDateTime)other == this;

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ HasDate.GetHashCode();
                hashCode = (hashCode * 397) ^ AsUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ (TzId != null ? TzId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString() => ToString(null, null);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var tz = TimeZoneName;
            if (!string.IsNullOrEmpty(tz))
            {
                tz = " " + tz;
            }

            if (format != null)
            {
                return Value.ToString(format, formatProvider) + tz;
            }
            if (HasTime && HasDate)
            {
                return Value + tz;
            }
            if (HasTime)
            {
                return Value.TimeOfDay + tz;
            }
            return Value.ToString("d") + tz;
        }
    }
}