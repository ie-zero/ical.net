using Ical.Net.Collections;

namespace Ical.Net
{
    public interface ICalendarObjectList<T> :
        IGroupedCollection<T> where T : class, ICalendarObject
    {
        T this[int index] { get; }
    }
}