using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;

namespace Ical.Net
{
    // TODO: Consider creating a CalendarPropertyValue class

    public class CalendarPropertyList : IEnumerable<CalendarProperty>
    {
        private readonly ICalendarObject _parent;
        private readonly GroupedValueList<CalendarProperty, object> _list;

        public CalendarPropertyList(ICalendarObject parent)
        {
            _parent = parent;
            _list = new GroupedValueList<CalendarProperty, object>();
            _list.ItemAdded += ItemAdded;
        }

        private void ItemAdded(object sender, ItemProcessedEventArgs<CalendarProperty> e)
        {
            e.Item.Parent = _parent;
        }

        public CalendarProperty this[string name]
        {
            get
            {
                return ContainsKey(name) ? AllOf(name).FirstOrDefault() : null;
            }
        }

        public IEnumerable<CalendarProperty> AllOf(string group)
        {
            return _list.Values(group);
        }

        public bool ContainsKey(string group)
        {
            return _list.ContainsKey(group);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void Add(CalendarProperty property)
        {
            _list.Add(property);
        }

        public bool Remove(string group)
        {
            return _list.Remove(group);
        }

        public void Set(string group, object value)
        {
            _list.Set(group, value);
        }

        public void Set(string group, IEnumerable<object> values)
        {
            _list.Set(group, values);
        }

        public T Get<T>(string group)
        {
            return _list.Get<T>(group);
        }

        public IList<T> GetMany<T>(string group)
        {
            return _list.GetMany<T>(group);
        }

        public IEnumerator<CalendarProperty> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}