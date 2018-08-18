using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ical.Net.Components;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Ical.Net.Utility;

namespace Ical.Net
{
    // TODO: CalendarCollection class does not have any state other than the one encapsulated in the List<T>.
    //      Consider converting the methods to extension methods on IEnumerable<Calendar>

    /// <summary>
    /// Collection of iCalendars.
    /// </summary>
    public class CalendarCollection : List<Calendar>
    {    
        public static CalendarCollection Load(string calendarString)
        {
            using (var reader = new StringReader(calendarString))
            { 
                return Load(reader);
            }
        }

        /// <summary>
        /// Loads an <see cref="Calendar"/> from an open stream.
        /// </summary>
        /// <param name="stream">The stream from which to load the <see cref="Calendar"/> object</param>
        /// <returns>An <see cref="Calendar"/> object</returns>
        public static CalendarCollection Load(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return Load(reader);
            }
        }

        public static CalendarCollection Load(TextReader reader)
        {
            var calendars = SimpleDeserializer.Default.Deserialize(reader).OfType<Calendar>();
            var collection = new CalendarCollection();
            collection.AddRange(calendars);
            return collection;
        }

        public void ClearEvaluation()
        {
            foreach (var calendar in this)
            {
                calendar.ClearEvaluation();
            }
        }

        public FreeBusy GetFreeBusy(FreeBusy freeBusyRequest)
        {
            return this.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(freeBusyRequest)));
        }

        public FreeBusy GetFreeBusy(IDateTime fromInclusive, IDateTime toExclusive)
        {
            return this.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(fromInclusive, toExclusive)));
        }

        public FreeBusy GetFreeBusy(Organizer organizer, IEnumerable<Attendee> contacts, IDateTime fromInclusive, IDateTime toExclusive)
        {
            return this.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(organizer, contacts, fromInclusive, toExclusive)));
        }

        private FreeBusy CombineFreeBusy(FreeBusy main, FreeBusy current)
        {
            // TODO: The implementation of the CombineFreeBusy() method is wrong. 
            //      The items are merged on the 'main' but the 'current' object is returned.

            main?.MergeWith(current);
            return current;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CalendarCollection)obj);
        }

        protected bool Equals(CalendarCollection obj)
        {
            return CollectionHelpers.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return CollectionHelpers.GetHashCode(this);
        }

        public HashSet<Occurrence> GetOccurrences(IDateTime dt)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(dt));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences(DateTime dt)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(dt));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences(IDateTime startTime, IDateTime endTime)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(startTime, endTime));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences(DateTime startTime, DateTime endTime)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(startTime, endTime));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences<T>(IDateTime dt) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(dt));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences<T>(DateTime dt) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(dt));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences<T>(IDateTime startTime, IDateTime endTime) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(startTime, endTime));
            }
            return occurrences;
        }

        public HashSet<Occurrence> GetOccurrences<T>(DateTime startTime, DateTime endTime) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in this)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(startTime, endTime));
            }
            return occurrences;
        }


    }
}