using System;
using System.IO;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents an RFC 5545 "BYDAY" value.
    /// </summary>
    public class WeekDay : EncodableDataType
    {
        public WeekDay()
        {
            Offset = int.MinValue;
        }

        public WeekDay(DayOfWeek day) : this()
        {
            DayOfWeek = day;
        }

        public WeekDay(DayOfWeek day, int num) : this(day)
        {
            Offset = num;
        }

        public WeekDay(DayOfWeek day, FrequencyOccurrence type) : this(day, (int)type) { }

        public WeekDay(string value)
        {
            var serializer = new WeekDaySerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public DayOfWeek DayOfWeek { get; set; }

        public int Offset { get; set; } = int.MinValue;

        public int CompareTo(object obj)
        {
            WeekDay bd = null;
            if (obj is string)
            {
                bd = new WeekDay(obj.ToString());
            }
            else if (obj is WeekDay)
            {
                bd = (WeekDay)obj;
            }

            if (bd == null)
            {
                throw new ArgumentException();
            }
            var compare = DayOfWeek.CompareTo(bd.DayOfWeek);
            if (compare == 0)
            {
                compare = Offset.CompareTo(bd.Offset);
            }
            return compare;
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var weekDay = copyable as WeekDay;
            if (weekDay == null) { return; }

            CopyFrom(weekDay);
        }

        private void CopyFrom(WeekDay weekDay)
        {
            Offset = weekDay.Offset;
            DayOfWeek = weekDay.DayOfWeek;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WeekDay))
            {
                return false;
            }

            var ds = (WeekDay) obj;
            return ds.Offset == Offset && ds.DayOfWeek == DayOfWeek;
        }

        public override int GetHashCode()
        {
            return Offset.GetHashCode() ^ DayOfWeek.GetHashCode();
        }
    }
}