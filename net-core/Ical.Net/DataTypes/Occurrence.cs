using System;
using Ical.Net.CalendarComponents;

namespace Ical.Net.DataTypes
{
    public class Occurrence : IComparable<Occurrence>
    {
        public Occurrence(Occurrence occurrence)
        {
            Period = occurrence.Period;
            Source = occurrence.Source;
        }

        public Occurrence(IRecurrable recurrable, Period period)
        {
            Source = recurrable;
            Period = period;
        }

        public Period Period { get; set; }

        public IRecurrable Source { get; set; }

        public int CompareTo(Occurrence other)
        {
            return Period.CompareTo(other.Period);
        }

        public bool Equals(Occurrence other)
        {
            return Equals(Period, other.Period) && Equals(Source, other.Source);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is Occurrence && Equals((Occurrence) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Period?.GetHashCode() ?? 0) * 397) ^ (Source?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            var output = "Occurrence";
            if (Source != null)
            {
                output = Source.GetType().Name + " ";
            }

            if (Period != null)
            {
                output += "(" + Period.StartTime + ")";
            }

            return output;
        }
    }
}