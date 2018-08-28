using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;
using Ical.Net.Collections.Proxies;

namespace Ical.Net.Proxies
{
    public class CalendarObjectListProxy<T> : ICalendarObjectList<T>
        where T : class, ICalendarObject
    {
        readonly GroupedCollectionProxy<ICalendarObject, T> _list;

        public CalendarObjectListProxy(IGroupedCollection<ICalendarObject> list)
        {
            _list = new GroupedCollectionProxy<ICalendarObject, T>(list);
        }

        public T this[int index]
        {
            get { return _list.Skip(index).FirstOrDefault(); }
        }

        public int Count
        {
            get { return _list.Count(); }
        }

        public bool IsReadOnly => throw new NotImplementedException(); 

        public event EventHandler<ItemProcessedEventArgs<T>> ItemAdded;

        public void Add(T item)
        {
            _list.Add(item);
        }

        public void Clear(string group)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string group)
        {
            return _list.Contains(group);
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
            return _list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}