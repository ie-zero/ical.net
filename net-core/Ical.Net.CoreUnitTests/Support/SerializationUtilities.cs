using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;

namespace Ical.Net.Tests.Support
{
    internal static class SerializationUtilities
    {
        public static string SerializeCalendar(Calendar calendar)
        {
            return new CalendarSerializer(SerializationContext.Default).SerializeToString(calendar);
        }

        public static string SerializeEvent(CalendarEvent calendarEvent)
        {
            return SerializeCalendar(new Calendar { Events = { calendarEvent } });
        }

        public static Calendar DeserializeCalendar(string calendarString)
        {
            return SimpleDeserializer.Default.Deserialize(new StringReader(calendarString)).Cast<Calendar>().Single();
        }

        public static IEnumerable<Calendar> DeserializeCalendars(string calendarString)
        {
            return SimpleDeserializer.Default.Deserialize(new StringReader(calendarString)).Cast<Calendar>().ToList();
        }
    }
}
