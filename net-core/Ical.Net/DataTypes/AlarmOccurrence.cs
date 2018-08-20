using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Components;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// Represents a specific occurrence of an <see cref="Alarm"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="AlarmOccurrence"/> contains the date/time when the alarm occurs,
    /// the <see cref="Alarm"/> that fired, and the component on which the alarm fired.
    /// </remarks>
    public class AlarmOccurrence : IComparable<AlarmOccurrence>
    {
        public AlarmOccurrence(AlarmOccurrence ao)
        {
            DateTime = ao.DateTime;
            Component = ao.Component;
            Alarm = ao.Alarm;
        }

        public AlarmOccurrence(Alarm alarm, IDateTime dt, IRecurringComponent rc)
        {
            Alarm = alarm;
            DateTime = dt;
            Component = rc;
        }

        public Alarm Alarm { get; }

        public IRecurringComponent Component { get; }

        public IDateTime DateTime { get; }

        public int CompareTo(AlarmOccurrence other)
        {
            return DateTime.CompareTo(other.DateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            var occurrence = (AlarmOccurrence)obj;

            return GetEqualityComponents().SequenceEqual(occurrence.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        protected IEnumerable<object> GetEqualityComponents()
        {
            // TODO: Alarm does not implement Equals or GetHashCode()
            yield return Alarm;
            yield return Component;
            yield return DateTime;
        }
    }
}