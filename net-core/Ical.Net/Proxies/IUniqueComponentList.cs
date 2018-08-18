using System.Collections.Generic;
using Ical.Net.Components;

namespace Ical.Net.Proxies
{
    public interface IUniqueComponentList<T> : ICalendarObjectList<T> where T : class, IUniqueComponent
    {
        T this[string uid] { get; }

        void AddRange(IEnumerable<T> collection);
    }
}