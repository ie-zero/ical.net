using Ical.Net.CalendarComponents;

namespace Ical.Net.Serialization
{
    public class CalendarComponentFactory
    {
        public ICalendarComponent Build(string objectName)
        {
            var name = objectName.ToUpper();

            switch (name)
            {
                case ComponentName.Alarm:
                    return new Alarm();

                case ComponentName.Event:
                    return new CalendarEvent();

                case ComponentName.Freebusy:
                    return new FreeBusy();

                case ComponentName.Journal:
                    return new Journal();

                case ComponentName.Timezone:
                    return new VTimeZone();

                case ComponentName.Todo:
                    return new Todo();

                case ComponentName.Calendar:
                    return new Calendar();

                default:
                    return new CalendarComponent(name);
            }
        }
    }
}