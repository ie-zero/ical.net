using Ical.Net.Collections;

namespace Ical.Net
{
    /// <summary>
    /// A collection of calendar objects.
    /// </summary>
    public class CalendarObjectList : GroupedList<ICalendarObject>, ICalendarObjectList<ICalendarObject>
    {
        public CalendarObjectList(ICalendarObject parent) {}
    }
}