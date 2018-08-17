using System.Collections.Generic;
using Ical.Net.CalendarComponents;

namespace Ical.Net.Proxies
{
    public interface IUniqueComponentList<T> : ICalendarObjectList<T> where T : class, IUniqueComponent
    {
        T this[string uid] { get; set; }

        void AddRange(IEnumerable<T> collection);
    }
}