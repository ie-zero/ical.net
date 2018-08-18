using Ical.Net.Components;
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
    }
}
