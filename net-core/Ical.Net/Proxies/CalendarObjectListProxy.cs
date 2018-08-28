using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;

namespace Ical.Net.Proxies
{
    public class CalendarObjectListProxy<T> : ICalendarObjectList<T>
        where T : class, ICalendarObject
    {
        private readonly GroupedCollection<string, ICalendarObject> _realObject;

        public CalendarObjectListProxy(IGroupedCollection<string, ICalendarObject> realObject)
        {
            _realObject = (GroupedCollection<string, ICalendarObject>)realObject;
        }

        public T this[int index]
        {
            get { return (T)_realObject.Skip(index).FirstOrDefault(); }
        }

        public int Count
        {
            get { return _realObject.OfType<T>().Count(); }
        }

        public event EventHandler<ItemProcessedEventArgs<T>> ItemAdded;

        public void Add(T item)
        {
            _realObject.Add(item);
        }

        public void Clear()
        {
            var items = _realObject
                .OfType<T>()
                .ToArray();

            foreach (var item in items)
            {
                _realObject.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            return _realObject.Contains(item);
        }

        public bool Contains(string group)
        {
            return _realObject.Contains(group);
        }

        public IEnumerable<T> Values(string group)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int CountOf(string group)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string group)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return _realObject.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _realObject.OfType<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}