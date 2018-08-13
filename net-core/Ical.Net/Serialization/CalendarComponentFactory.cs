﻿using Ical.Net.CalendarComponents;

namespace Ical.Net.Serialization
{
    public class CalendarComponentFactory
    {
        public ICalendarComponent Build(string objectName)
        {
            ICalendarComponent c;
            var name = objectName.ToUpper();

            switch (name)
            {
                case Components.Alarm:
                    c = new Alarm();
                    break;
                case EventStatus.Name:
                    c = new CalendarEvent();
                    break;
                case Components.Freebusy:
                    c = new FreeBusy();
                    break;
                case JournalStatus.Name:
                    c = new Journal();
                    break;
                case Components.Timezone:
                    c = new VTimeZone();
                    break;
                case TodoStatus.Name:
                    c = new Todo();
                    break;
                case Components.Calendar:
                    c = new Calendar();
                    break;
                default:
                    c = new CalendarComponent(name);
                    break;
            }
            return c;
        }
    }
}