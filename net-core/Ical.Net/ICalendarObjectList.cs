using Ical.Net.Collections;

namespace Ical.Net
{
    public interface ICalendarObjectList<T> : IGroupedCollection<string, T> where T : class, ICalendarObject
    {
        T this[int index] { get; }

        void Clear();
    }
}