using System;
using System.IO;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;
using Ical.Net.Utility;
using NodaTime;

namespace Ical.Net.DataTypes
{
    // TODO: CalDateTime class is presented as a value object but it is not as it inherits from a non-value object.  

    /// <summary>
    /// The iCalendar equivalent of the .NET <see cref="DateTime"/> class.
    /// <remarks>
    /// In addition to the features of the <see cref="DateTime"/> class, the <see cref="CalDateTime"/>
    /// class handles time zone differences, and integrates seamlessly into the iCalendar framework.
    /// </remarks>
    /// </summary>
    public sealed class CalDateTime : EncodableDataType, IDateTime, IComparable<IDateTime>
    {
        private DateTime _asUtc = DateTime.MinValue;
        private string _tzId = string.Empty;
        private DateTime _value;

        public CalDateTime() { }

        public CalDateTime(IDateTime value)
        {
            Initialize(value.Value, value.TzId, null);
        }

        public CalDateTime(DateTime value) : this(value, null) { }

        /// <summary>
        /// Specifying a <paramref name="tzId"/> value will override <paramref name="value"/>'s
        /// 'DateTimeKind' property. If the time zone specified is UTC, the underlying 'DateTimeKind'
        /// will be 'Utc'. If a non-UTC time zone is specified, the underlying 'DateTimeKind'
        /// property will be 'Local'. If no time zone is specified, the 'DateTimeKind' property will
        /// be left untouched.
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
            var serializer = new DateTimeSerializer(new SerializationContext());
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public static CalDateTime Now => new CalDateTime(DateTime.Now);

        public static CalDateTime Today => new CalDateTime(DateTime.Today);

        public DateTime Date => Value.Date;

        public int Day => Value.Day;

        public DayOfWeek DayOfWeek => Value.DayOfWeek;

        public int DayOfYear => Value.DayOfYear;

        public bool HasDate { get; set; }

        public bool HasTime { get; set; }

        public int Hour => Value.Hour;

        public bool IsUtc
        {
            get { return _value.Kind == DateTimeKind.Utc; }
        }

        public int Millisecond => Value.Millisecond;

        public int Minute => Value.Minute;

        public int Month => Value.Month;

        public int Second => Value.Second;

        public long Ticks => Value.Ticks;

        public TimeSpan TimeOfDay => Value.TimeOfDay;

        public string TimeZoneName => TzId;

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

        public int Year => Value.Year;

        public static implicit operator CalDateTime(DateTime left)
        {
            return new CalDateTime(left);
        }

        public static TimeSpan operator -(CalDateTime left, IDateTime right)
        {
            return left.GetAsUtc() - right.GetAsUtc();
        }

        public static IDateTime operator -(CalDateTime left, TimeSpan right)
        {
            var copy = left.Copy<IDateTime>();
            copy.Value -= right;
            return copy;
        }

        public static bool operator !=(CalDateTime left, IDateTime right)
        {
            return !(left == right);
        }

        public static IDateTime operator +(CalDateTime left, TimeSpan right)
        {
            var copy = left.Copy<IDateTime>();
            copy.Value += right;
            return copy;
        }

        public static bool operator <(CalDateTime left, IDateTime right)
        {
            return left != null && right != null && left.GetAsUtc() < right.GetAsUtc();
        }

        public static bool operator <=(CalDateTime left, IDateTime right)
        {
            return left != null && right != null && left.GetAsUtc() <= right.GetAsUtc();
        }

        public static bool operator ==(CalDateTime left, IDateTime right)
        {
            return ReferenceEquals(left, null) || ReferenceEquals(right, null)
                ? ReferenceEquals(left, right)
                : right is CalDateTime
                    && left.Value.Equals(right.Value)
                    && left.HasDate == right.HasDate
                    && left.GetAsUtc().Equals(right.GetAsUtc())
                    && string.Equals(left.TzId, right.TzId, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator >(CalDateTime left, IDateTime right)
        {
            return left != null && right != null && left.GetAsUtc() > right.GetAsUtc();
        }

        public static bool operator >=(CalDateTime left, IDateTime right)
        {
            return left != null && right != null && left.GetAsUtc() >= right.GetAsUtc();
        }

        public IDateTime Add(TimeSpan ts)
        {
            return this + ts;
        }

        public IDateTime AddDays(int days)
        {
            var dt = Copy<IDateTime>();
            dt.Value = Value.AddDays(days);
            return dt;
        }

        public IDateTime AddHours(int hours)
        {
            var dt = Copy<IDateTime>();
            if (!dt.HasTime && hours % 24 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddHours(hours);
            return dt;
        }

        public IDateTime AddMilliseconds(int milliseconds)
        {
            var dt = Copy<IDateTime>();
            if (!dt.HasTime && milliseconds % 86400000 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddMilliseconds(milliseconds);
            return dt;
        }

        public IDateTime AddMinutes(int minutes)
        {
            var dt = Copy<IDateTime>();
            if (!dt.HasTime && minutes % 1440 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddMinutes(minutes);
            return dt;
        }

        public IDateTime AddMonths(int months)
        {
            var dt = Copy<IDateTime>();
            dt.Value = Value.AddMonths(months);
            return dt;
        }

        public IDateTime AddSeconds(int seconds)
        {
            var dt = Copy<IDateTime>();
            if (!dt.HasTime && seconds % 86400 > 0)
            {
                dt.HasTime = true;
            }
            dt.Value = Value.AddSeconds(seconds);
            return dt;
        }

        public IDateTime AddTicks(long ticks)
        {
            var dt = Copy<IDateTime>();
            dt.HasTime = true;
            dt.Value = Value.AddTicks(ticks);
            return dt;
        }

        public IDateTime AddYears(int years)
        {
            var dt = Copy<IDateTime>();
            dt.Value = Value.AddYears(years);
            return dt;
        }

        public void AssociateWith(IDateTime dt)
        {
            if (AssociatedObject == null && dt.AssociatedObject != null)
            {
                AssociatedObject = dt.AssociatedObject;
            }
            else if (AssociatedObject != null && dt.AssociatedObject == null)
            {
                dt.Associate(AssociatedObject);
            }
        }

        /// <summary>
        /// Returns a representation of the DateTime in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime GetAsUtc()
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
                else if (IsUtc || Value.Kind == DateTimeKind.Utc)
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

        /// <summary>
        /// Converts the date/time to the date/time of the computer running the program. If the
        /// DateTimeKind is Unspecified, it is assumed that the underlying value already represents
        /// the system's datetime.
        /// </summary>
        public DateTime GetAsSystemLocal()
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

        /// <summary>
        /// Returns a DateTimeOffset representation of the Value. If a TzId is specified, it will use
        /// that time zone's UTC offset, otherwise it will use the system-local time zone.
        /// </summary>
        public DateTimeOffset GetAsDateTimeOffset()
        {
            return string.IsNullOrWhiteSpace(TzId)
                ? new DateTimeOffset(GetAsSystemLocal())
                : DateUtil.ToZonedDateTimeLeniently(Value, TzId).ToDateTimeOffset();
        }

        public int CompareTo(IDateTime other)
        {
            if (other == null) { return 1; }
            if (this < other) { return -1; }
            if (this == other) { return 0; }
            return 1;
        }

        public IDateTime Copy()
        {
            var value = new CalDateTime();
            value.CopyFrom(this);

            return value;
        }

        public override void CopyFrom(ICopyable obj)
        {
            base.CopyFrom(obj);

            var dt = obj as IDateTime;
            if (dt == null)
            {
                return;
            }

            _value = dt.Value;
            HasDate = dt.HasDate;
            HasTime = dt.HasTime;

            AssociateWith(dt);
        }

        public bool Equals(CalDateTime other)
        {
            return this == other;
        }

        public override bool Equals(object other)
        {
            return other is IDateTime && (CalDateTime)other == this;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ HasDate.GetHashCode();
                hashCode = (hashCode * 397) ^ GetAsUtc().GetHashCode();
                hashCode = (hashCode * 397) ^ (TzId != null ? TzId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool GreaterThan(IDateTime dt)
        {
            return this > dt;
        }

        public bool GreaterThanOrEqual(IDateTime dt)
        {
            return this >= dt;
        }

        public bool LessThan(IDateTime dt)
        {
            return this < dt;
        }

        public bool LessThanOrEqual(IDateTime dt)
        {
            return this <= dt;
        }

        public IDateTime Subtract(TimeSpan ts)
        {
            return this - ts;
        }

        public TimeSpan Subtract(IDateTime dt)
        {
            return this - dt;
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

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

        /// <summary>
        /// Returns a representation of the IDateTime in the specified time zone
        /// </summary>
        public IDateTime ToTimeZone(string tzId)
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
            catch { }

            return dt;
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
    }
}