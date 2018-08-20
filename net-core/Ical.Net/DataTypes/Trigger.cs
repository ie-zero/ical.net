using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ical.Net.Serialization;
using Ical.Net.Serialization.DataTypes;

namespace Ical.Net.DataTypes
{
    /// <summary>
    /// A class that is used to specify exactly when an <see cref="ComponentName.Alarm"/> component will trigger.
    /// Usually this date/time is relative to the component to which the Alarm is associated.
    /// </summary>    
    public class Trigger : EncodableDataType
    {
        private IDateTime _dateTime;
        private TimeSpan? _duration;

        public Trigger() { }

        public Trigger(TimeSpan ts)
        {
            Duration = ts;
        }

        public Trigger(string value) : this()
        {
            var serializer = new TriggerSerializer(SerializationContext.Default);
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        public IDateTime DateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                if (_dateTime == null)
                {
                    return;
                }

                // NOTE: this, along with the "Duration" setter, fixes the bug tested in
                // TODO11(), as well as this thread: https://sourceforge.net/forum/forum.php?thread_id=1926742&forum_id=656447

                // DateTime and Duration are mutually exclusive
                Duration = null;

                // Do not allow timeless date/time values
                _dateTime.HasTime = true;
            }
        }

        public TimeSpan? Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                if (_duration != null)
                {
                    // NOTE: see above.

                    // DateTime and Duration are mutually exclusive
                    DateTime = null;
                }
            }
        }

        public bool IsRelative
        {
            get { return _duration != null; }
        }

        public string Related { get; set; } = TriggerRelation.Start;

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var trigger = copyable as Trigger;
            if (trigger == null) { return; }

            CopyFrom(trigger);
        }

        private void CopyFrom(Trigger trigger)
        {
            DateTime = trigger.DateTime;
            Duration = trigger.Duration;
            Related = trigger.Related;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            var trigger = (Trigger)obj;

            return GetEqualityComponents().SequenceEqual(trigger.GetEqualityComponents());
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
            yield return _dateTime;
            yield return _duration;
            yield return Related;
        }
    }
}