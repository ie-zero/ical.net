using System;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents a time offset from UTC (Coordinated Universal Time).
    /// </summary>
    public class UtcOffset : EncodableDataType
    {
        public UtcOffset(string value)
        {
            Offset = UtcOffsetSerializer.GetOffset(value);
        }

        public UtcOffset(TimeSpan ts)
        {
            Offset = ts;
        }

        public TimeSpan Offset { get; }

        public bool Positive
        {
            get { return Offset >= TimeSpan.Zero; }
        }

        public int Hours
        {
            get { return Math.Abs(Offset.Hours); }
        }

        public int Minutes
        {
            get { return Math.Abs(Offset.Minutes); }
        }

        public int Seconds
        {
            get { return Math.Abs(Offset.Seconds); }
        }

        public static explicit operator TimeSpan(UtcOffset o)
        {
            return o.Offset;
        }

        public static implicit operator UtcOffset(TimeSpan ts)
        {
            return new UtcOffset(ts);
        }

        protected bool Equals(UtcOffset other)
        {
            return Offset == other.Offset;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((UtcOffset)obj);
        }

        public override int GetHashCode()
        {
            return Offset.GetHashCode();
        }

        public override string ToString()
        {
            return (Positive ? "+" : "-") + Hours.ToString("00") + Minutes.ToString("00") + (Seconds != 0 ? Seconds.ToString("00") : string.Empty);
        }
    }
}