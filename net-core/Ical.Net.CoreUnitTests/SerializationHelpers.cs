﻿using Ical.Net.Components;
using Ical.Net.Serialization;

namespace Ical.Net.CoreUnitTests
{
    internal class SerializationHelpers
    {
        public static string SerializeToString(CalendarEvent calendarEvent)
            => SerializeToString(new Calendar { Events = { calendarEvent } });

        public static string SerializeToString(Calendar iCalendar)
            => new CalendarSerializer(new SerializationContext()).SerializeToString(iCalendar);
    }
}
