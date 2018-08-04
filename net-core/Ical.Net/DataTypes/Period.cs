using System;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary> Represents an iCalendar period of time. </summary>    
    public class Period : EncodableDataType, IComparable<Period>
    {
        private TimeSpan _duration;
        private IDateTime _endTime;
        private IDateTime _startTime;

        public Period() { }

        public Period(IDateTime occurs) : this(occurs, default(TimeSpan)) {}

        public Period(IDateTime start, IDateTime end)
        {
            if (end != null && end.LessThanOrEqual(start))
            {
                // TODO: Improve thrown exception.
                throw new ArgumentException($"Start time ( {start} ) must come before the end time ( {end} )");
            }

            StartTime = start;
            if (end == null)
            {
                return;
            }
            EndTime = end;
            Duration = end.Subtract(start);
        }

        public Period(IDateTime start, TimeSpan duration)
        {
            if (duration < TimeSpan.Zero)
            {
                // TODO: Improve thrown exception.
                throw new ArgumentException($"Duration ( ${duration} ) cannot be less than zero");
            }

            StartTime = start;
            if (duration == default(TimeSpan))
            {
                return;
            }

            Duration = duration;
            EndTime = start.Add(duration);
        }

        public TimeSpan Duration
        {
            get
            {
                if (StartTime != null
                    && EndTime == null
                    && StartTime.Value.TimeOfDay == TimeSpan.Zero)
                {
                    return TimeSpan.FromDays(1);
                }
                return _duration;
            }

            set
            {
                if (Equals(_duration, value))
                {
                    return;
                }
                _duration = value;
                ExtrapolateTimes();
            }
        }

        public IDateTime EndTime
        {
            get => _endTime;
            set
            {
                if (Equals(_endTime, value))
                {
                    return;
                }
                _endTime = value;
                ExtrapolateTimes();
            }
        }

        public IDateTime StartTime
        {
            get
            {
                return _startTime.HasTime
                    ? _startTime
                    : new CalDateTime(new DateTime(_startTime.Value.Year, _startTime.Value.Month, _startTime.Value.Day, 0, 0, 0), _startTime.TzId);
            }

            set
            {
                if (Equals(_startTime, value))
                {
                    return;
                }
                _startTime = value;
                ExtrapolateTimes();
            }
        }

        public bool CollidesWith(Period period)
        {
            return period != null
                && ((period.StartTime != null && Contains(period.StartTime)) || (period.EndTime != null && Contains(period.EndTime)));
        }

        public int CompareTo(Period other)
        {
            if (StartTime.Equals(other.StartTime))
            {
                return 0;
            }
            if (StartTime.LessThan(other.StartTime))
            {
                return -1;
            }
            if (StartTime.GreaterThan(other.StartTime))
            {
                return 1;
            }
            throw new Exception("An error occurred while comparing two Periods.");
        }

        public bool Contains(IDateTime dt)
        {
            // Start time is inclusive
            if (dt == null || StartTime == null || !StartTime.LessThanOrEqual(dt))
            {
                return false;
            }

            // End time is exclusive
            return EndTime == null || EndTime.GreaterThan(dt);
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var period = copyable as Period;
            if (period == null) { return; }

            StartTime = period.StartTime;
            EndTime = period.EndTime;
            Duration = period.Duration;
        }

        protected bool Equals(Period other)
        {
            return Equals(StartTime, other.StartTime) && Equals(EndTime, other.EndTime) && Duration.Equals(other.Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Period)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StartTime?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (EndTime?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            var periodSerializer = new PeriodSerializer();
            return periodSerializer.SerializeToString(this);
        }

        private void ExtrapolateTimes()
        {
            if (EndTime == null && StartTime != null && Duration != default(TimeSpan))
            {
                EndTime = StartTime.Add(Duration);
            }
            else if (Duration == default(TimeSpan) && StartTime != null && EndTime != null)
            {
                Duration = EndTime.Subtract(StartTime);
            }
            else if (StartTime == null && Duration != default(TimeSpan) && EndTime != null)
            {
                StartTime = EndTime.Subtract(Duration);
            }
        }
    }
}