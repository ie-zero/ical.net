using Ical.Net.CalendarComponents;

namespace Ical.Net.Serialization
{
    public class CalendarComponentFactory
    {
        public virtual ICalendarComponent Build(string objectName)
        {
            switch (objectName.ToUpper())
            {
                case Components.Alarm:
                    return new Alarm();
  
                case EventStatus.Name:
                    return new CalendarEvent();
                    
                case Components.Freebusy:
                    return new FreeBusy();
                    
                case JournalStatus.Name:
                    return new Journal();
                    
                case Components.Timezone:
                    return new VTimeZone();
                    
                case TodoStatus.Name:
                    return new Todo();
                    
                case Components.Calendar:
                    return new Calendar();
                    
                default:
                    return new CalendarComponent(objectName.ToUpper());
            }
        }
    }
}