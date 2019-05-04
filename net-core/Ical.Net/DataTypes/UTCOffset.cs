using System;
using Ical.Net.DataTypes.Values;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents a time offset from UTC (Coordinated Universal Time).
    /// </summary>
    public class UtcOffset : EncodableDataType
    {
        private UtcOffsetValue _value;

        public TimeSpan Offset => _value;

        public bool Positive => _value.Positive;

        public int Hours => _value.Hours;

        public int Minutes => _value.Minutes;

        public int Seconds => _value.Seconds;

        public UtcOffset()
        {
            _value = UtcOffsetValue.Zero;
        }

        public UtcOffset(string value)
        {
            var offset = UtcOffsetSerializer.GetOffset(value);
            _value = new UtcOffsetValue(offset);
        }

        public UtcOffset(TimeSpan offset)
        {
            _value = new UtcOffsetValue(offset);
        }

        public static implicit operator UtcOffset(TimeSpan value)
        {
            return new UtcOffset(value);
        }

        public static explicit operator TimeSpan(UtcOffset offset)
        {
            return offset._value;
        }

        public virtual DateTime ToUtc(DateTime dt) => DateTime.SpecifyKind(dt.Add(-Offset), DateTimeKind.Utc);

        public virtual DateTime ToLocal(DateTime dt) => DateTime.SpecifyKind(dt.Add(Offset), DateTimeKind.Local);

        protected bool Equals(UtcOffset other) => Offset == other.Offset;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (ReferenceEquals(null, obj))
                return false;

            var other = obj as UtcOffset;

            if (other == null)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return (Positive ? "+" : "-") + Hours.ToString("00") + Minutes.ToString("00") + (Seconds != 0 ? Seconds.ToString("00") : string.Empty);
        }
    }
}