using System;
using Ical.Net.CalendarComponents;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// It represents a specific occurrence of an <see cref="Alarm"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="AlarmOccurrence"/> contains the <see cref="Period"/> when the alarm occurs,
    /// the <see cref="Alarm"/> that fired, and the component on which the alarm fired.
    /// </remarks>
    public class AlarmOccurrence : IComparable<AlarmOccurrence>
    {
        public AlarmOccurrence(AlarmOccurrence ao)
        {
            Period = ao.Period;
            Component = ao.Component;
            Alarm = ao.Alarm;
        }

        public AlarmOccurrence(Alarm alarm, IDateTime dt, IRecurringComponent rc)
        {
            Alarm = alarm;
            Period = new Period(dt);
            Component = rc;
        }

        public Alarm Alarm { get; set; }

        public IRecurringComponent Component { get; set; }

        public IDateTime DateTime
        {
            get => Period.StartTime;
            set => Period = new Period(value);
        }

        public Period Period { get; set; }

        public int CompareTo(AlarmOccurrence other)
        {
            return Period.CompareTo(other.Period);
        }

        protected bool Equals(AlarmOccurrence other)
        {
            return Equals(Period, other.Period)
                           && Equals(Component, other.Component)
                           && Equals(Alarm, other.Alarm);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AlarmOccurrence)obj);
        }

        public override int GetHashCode()
        {
            // TODO: Alarm does not implement Equals or GetHashCode()
            unchecked
            {
                var hashCode = Period?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Component?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Alarm?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}