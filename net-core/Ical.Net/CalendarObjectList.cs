using Ical.Net.Collections;

namespace Ical.Net
{
    /// <summary>
    /// A collection of calendar objects.
    /// </summary>
    public class CalendarObjectList : GroupedCollection<ICalendarObject>, ICalendarObjectList<ICalendarObject>
    {
        // TODO: Encapsualting the GroupedList<ICalendarObject> as readonly field for CalendarObjectList generates unexpected test failures.
    }
}