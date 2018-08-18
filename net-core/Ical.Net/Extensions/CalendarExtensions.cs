using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ical.Net.Components;
using Ical.Net.DataTypes;

namespace Ical.Net.Extensions
{
    public static class CalendarExtensions
    {
        public static HashSet<Occurrence> GetOccurrences(this IEnumerable<Calendar> calendars, IDateTime dt)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(dt));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences(this IEnumerable<Calendar> calendars, DateTime dt)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars.ToArray())
            {
                occurrences.UnionWith(occurrence.GetOccurrences(dt));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences(this IEnumerable<Calendar> calendars, IDateTime startTime, IDateTime endTime)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(startTime, endTime));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences(this IEnumerable<Calendar> calendars, DateTime startTime, DateTime endTime)
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences(startTime, endTime));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences<T>(this IEnumerable<Calendar> calendars, IDateTime dt) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(dt));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences<T>(this IEnumerable<Calendar> calendars, DateTime dt) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(dt));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences<T>(this IEnumerable<Calendar> calendars, IDateTime startTime, IDateTime endTime) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(startTime, endTime));
            }
            return occurrences;
        }

        public static HashSet<Occurrence> GetOccurrences<T>(this IEnumerable<Calendar> calendars, DateTime startTime, DateTime endTime) where T : IRecurringComponent
        {
            var occurrences = new HashSet<Occurrence>();
            foreach (var occurrence in calendars)
            {
                occurrences.UnionWith(occurrence.GetOccurrences<T>(startTime, endTime));
            }
            return occurrences;
        }

        public static FreeBusy GetFreeBusy(this IEnumerable<Calendar> calendars, FreeBusy freeBusyRequest)
        {
            return calendars.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(freeBusyRequest)));
        }

        public static FreeBusy GetFreeBusy(this IEnumerable<Calendar> calendars, IDateTime fromInclusive, IDateTime toExclusive)
        {
            return calendars.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(fromInclusive, toExclusive)));
        }

        public static FreeBusy GetFreeBusy(this IEnumerable<Calendar> calendars, Organizer organizer, IEnumerable<Attendee> contacts, IDateTime fromInclusive, IDateTime toExclusive)
        {
            return calendars.Aggregate<Calendar, FreeBusy>(null, (current, iCal) => CombineFreeBusy(current, iCal.GetFreeBusy(organizer, contacts, fromInclusive, toExclusive)));
        }

        private static FreeBusy CombineFreeBusy(FreeBusy main, FreeBusy current)
        {
            // TODO: The implementation of the CombineFreeBusy() method is wrong. 
            //      The items are merged on the 'main' but the 'current' object is returned.

            main?.MergeWith(current);
            return current;
        }
    }
}
