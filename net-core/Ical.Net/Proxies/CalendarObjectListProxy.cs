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
            _list.Clear(group);
        }

        public void Clear()
        {
            throw new NotImplementedException();
            _list.Clear();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
            return _list.Contains(item);
        }

        public bool ContainsKey(string group)
        {
            return _list.ContainsKey(group);
        }

        public IEnumerable<T> AllOf(string group)
        {
            throw new NotImplementedException();
            return _list.AllOf(group);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int CountOf(string group)
        {
            throw new NotImplementedException();
            return _list.CountOf(group);
        }

        public bool Remove(string group)
        {
            throw new NotImplementedException();
            return _list.Remove(group);
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