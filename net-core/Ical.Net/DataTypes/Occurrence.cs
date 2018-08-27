using System.Collections.Generic;
using System.Linq;
using Ical.Net.Components;

namespace Ical.Net.DataTypes
{
    public class Occurrence
    {
        public Occurrence(Occurrence occurrence)
        {
            Period = occurrence.Period;
            Source = occurrence.Source;
        }

        public Occurrence(IRecurrable source, Period period)
        {
            Source = source;
            Period = period;
        }

        public IRecurrable Source { get; }

        public Period Period { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            var occurrence = (Occurrence)obj;

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
            yield return Source;
            yield return Period;
        }
    }
}