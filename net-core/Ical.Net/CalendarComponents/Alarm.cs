using System;
using System.Collections.Generic;
using Ical.Net.DataTypes;

namespace Ical.Net.CalendarComponents
{
    /// <summary>
    /// A class that represents an RFC 2445 VALARM component.  
    /// </summary>    
    public class Alarm : CalendarComponent
    {
        public Alarm() : base(ComponentName.Alarm)
        {
            Occurrences = new List<AlarmOccurrence>();
        }

        public string Action
        {
            get => Properties.Get<string>(AlarmAction.Key);
            set => Properties.Set(AlarmAction.Key, value);
        }

        public Attachment Attachment
        {
            get => Properties.Get<Attachment>("ATTACH");
            set => Properties.Set("ATTACH", value);
        }

        public IList<Attendee> Attendees
        {
            get => Properties.GetMany<Attendee>("ATTENDEE");
            set => Properties.Set("ATTENDEE", value);
        }

        public string Description
        {
            get => Properties.Get<string>("DESCRIPTION");
            set => Properties.Set("DESCRIPTION", value);
        }

        public TimeSpan Duration
        {
            get => Properties.Get<TimeSpan>("DURATION");
            set => Properties.Set("DURATION", value);
        }

        public int Repeat
        {
            get => Properties.Get<int>("REPEAT");
            set => Properties.Set("REPEAT", value);
        }

        public string Summary
        {
            get => Properties.Get<string>("SUMMARY");
            set => Properties.Set("SUMMARY", value);
        }

        public Trigger Trigger
        {
            get => Properties.Get<Trigger>(TriggerRelation.Key);
            set => Properties.Set(TriggerRelation.Key, value);
        }

        protected IList<AlarmOccurrence> Occurrences { get; set; }

        /// <summary>
        /// Gets a list of alarm occurrences for the given recurring component, <paramref name="rc"/>
        /// that occur between <paramref name="fromDate"/> and <paramref name="toDate"/>.
        /// </summary>
        public IList<AlarmOccurrence> GetOccurrences(IRecurringComponent rc, IDateTime fromDate, IDateTime toDate)
        {
            // TODO: Move GetOccurrences() logic into an AlarmEvaluator.

            Occurrences.Clear();

            if (Trigger == null)
            {
                return Occurrences;
            }

            // If the trigger is relative, it can recur right along with
            // the recurring items, otherwise, it happens once and
            // only once (at a precise time).
            if (Trigger.IsRelative)
            {
                // Ensure that "FromDate" has already been set
                if (fromDate == null)
                {
                    fromDate = rc.Start.Copy();
                }

                var d = default(TimeSpan);
                foreach (var o in rc.GetOccurrences(fromDate, toDate))
                {
                    var dt = o.Period.StartTime;
                    if (string.Equals(Trigger.Related, TriggerRelation.End, TriggerRelation.Comparison))
                    {
                        if (o.Period.EndTime != null)
                        {
                            dt = o.Period.EndTime;
                            if (d == default(TimeSpan))
                            {
                                d = o.Period.Duration;
                            }
                        }
                        // Use the "last-found" duration as a reference point
                        else if (d != default(TimeSpan))
                        {
                            dt = o.Period.StartTime.Add(d);
                        }
                        else
                        {
                            throw new ArgumentException(
                                "Alarm trigger is relative to the START of the occurrence; however, the occurence has no discernible end.");
                        }
                    }

                    Occurrences.Add(new AlarmOccurrence(this, dt.Add(Trigger.Duration.Value), rc));
                }
            }
            else
            {
                var dt = Trigger.DateTime.Copy();
                dt.AssociatedObject = this;
                Occurrences.Add(new AlarmOccurrence(this, dt, rc));
            }

            // If a REPEAT and DURATION value were specified,
            // then handle those repetitions here.
            AddRepeatedItems();

            return Occurrences;
        }

        /// <summary>
        /// Polls the <see cref="Alarm"/> component for alarms that have been triggered since the
        /// provided <paramref name="start"/> date/time.
        ///
        /// If <paramref name="start"/> is null, all triggered alarms will be returned.
        /// </summary>
        /// <param name="start">The earliest date/time to poll trigered alarms for.</param>
        /// <returns>
        /// A list of <see cref="AlarmOccurrence"/> objects, each containing a triggered alarm.
        /// </returns>
        public IList<AlarmOccurrence> Poll(IDateTime start, IDateTime end)
        {
            var results = new List<AlarmOccurrence>();

            // Evaluate the alarms to determine the recurrences
            var rc = Parent as RecurringComponent;
            if (rc == null)
            {
                return results;
            }

            results.AddRange(GetOccurrences(rc, start, end));
            return results;
        }

        /// <summary>
        /// Handles the repetitions that occur from the <c>REPEAT</c> and <c>DURATION</c> properties.
        /// Each recurrence of the alarm will have its own set of generated repetitions.
        /// </summary>
        protected void AddRepeatedItems()
        {
            var len = Occurrences.Count;
            for (var index = 0; index < len; index++)
            {
                var ao = Occurrences[index];
                var alarmTime = ao.DateTime.Copy();

                for (var j = 0; j < Repeat; j++)
                {
                    alarmTime = alarmTime.Add(Duration);
                    Occurrences.Add(new AlarmOccurrence(this, alarmTime.Copy(), ao.Component));
                }
            }
        }
    }
}