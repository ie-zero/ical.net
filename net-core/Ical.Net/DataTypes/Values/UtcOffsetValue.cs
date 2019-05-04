using System;
using System.Collections.Generic;

namespace Ical.Net.DataTypes.Values
{
    /// <summary>
    /// Represents a time offset from UTC (Coordinated Universal Time).
    /// </summary>
    public sealed class UtcOffsetValue : ValueObject
    {
        // 
        // Note: The order of appearance in the code matters for the initialisation of the static fields when they 
        //      reference each other. 
        // 

        private readonly static TimeSpan MinimumTimeSpan = new TimeSpan(-12, 0, 0);
        private readonly static TimeSpan MaximumTimeSpan = new TimeSpan(14, 0, 0);
        private readonly static TimeSpan PrecisionTimeSpan = new TimeSpan(0, 15, 0);

        public readonly static UtcOffsetValue Minimum = new UtcOffsetValue(MinimumTimeSpan);
        public readonly static UtcOffsetValue Maximum = new UtcOffsetValue(MaximumTimeSpan);
        public readonly static UtcOffsetValue Zero = new UtcOffsetValue(TimeSpan.Zero);

        private readonly TimeSpan _value;

        public UtcOffsetValue(int hours, int minutes) : this(new TimeSpan(hours, minutes, 0))
        {
        }

        public UtcOffsetValue(TimeSpan offset)
        {
            if (offset < MinimumTimeSpan || offset > MaximumTimeSpan)
                throw new ArgumentException(nameof(offset), "UTC offset cannot be less than -12 hours or more than 14 hours");

            //if ((offset.Ticks % PrecisionTimeSpan.Ticks) != 0)
            //    throw new ArgumentException(nameof(offset), "UTC offset can only increase in 15 minute intervals");

            _value = offset;
        }

        public bool Positive
        {
            get { return _value >= TimeSpan.Zero; }
        }

        public int Hours
        {
            get { return Math.Abs(_value.Hours); }
        }

        public int Minutes
        {
            get { return Math.Abs(_value.Minutes); }
        }

        public int Seconds
        {
            get { return Math.Abs(_value.Seconds); }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }

        public static implicit operator TimeSpan(UtcOffsetValue offset)
        {
            return offset._value;
        }

        public static explicit operator UtcOffsetValue(TimeSpan value)
        {
            return new UtcOffsetValue(value);
        }

        public static bool IsValid(int hours, int minutes)
        {
            return IsValid(new TimeSpan(hours, minutes, 0));
        }

        public static bool IsValid(TimeSpan offset)
        {
            if (offset < MinimumTimeSpan || offset > MaximumTimeSpan)
                return false;

            //if ((offset.Ticks % PrecisionTimeSpan.Ticks) != 0)
            //    return false;

            return true;
        }

        // TODO: Implement IComperable
        // TODO: Implement >, < operators
    }
}
