using Ical.Net.CalendarComponents;

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
                case ComponentName.Alarm:
                    c = new Alarm();
                    break;
                case ComponentName.Event:
                    c = new CalendarEvent();
                    break;
                case ComponentName.Freebusy:
                    c = new FreeBusy();
                    break;
                case ComponentName.Journal:
                    c = new Journal();
                    break;
                case ComponentName.Timezone:
                    c = new VTimeZone();
                    break;
                case ComponentName.Todo:
                    c = new Todo();
                    break;
                case ComponentName.Calendar:
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