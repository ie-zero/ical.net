using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;

namespace Ical.Net.IntegrationTests
{
    internal class SerializationHelpers
    {
        public static string SerializeToString(CalendarEvent calendarEvent)
            => SerializeToString(new Calendar { Events = { calendarEvent } });

        public static string SerializeToString(Calendar iCalendar)
            => new CalendarSerializer(SerializationContext.Default).SerializeToString(iCalendar);
    }
}
