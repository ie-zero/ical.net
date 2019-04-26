using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;

namespace Ical.Net
{
    public class CalendarPropertyList : IEnumerable<ICalendarProperty>
    {
        private readonly ICalendarObject _parent;
        private readonly GroupedValueList<string, ICalendarProperty, CalendarProperty, object> _properties;

        public CalendarPropertyList(ICalendarObject parent)
        {
            _parent = parent;

            _properties = new GroupedValueList<string, ICalendarProperty, CalendarProperty, object>();
            _properties.ItemAdded += CalendarPropertyList_ItemAdded;
        }

        public ICalendarProperty this[string name]
        {
            get
            {
                return _properties.ContainsKey(name) ? _properties.AllOf(name).FirstOrDefault() : null;
            }
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public void Add(ICalendarProperty property)
        {
            _properties.Add(property);
        }

        public void Set(string name, object value)
        {
            _properties.Set(name, value);
        }

        public void Set(string name, IEnumerable<object> values)
        {
            _properties.Set(name, values);
        }

        public void Remove(string name)
        {
            _properties.Remove(name);
        }

        public bool ContainsKey(string name)
        {
            return _properties.ContainsKey(name);
        }

        public T Get<T>(string name)
        {
            return _properties.Get<T>(name);
        }

        public IList<T> GetMany<T>(string name)
        {
            return _properties.GetMany<T>(name);
        }

        // TODO: AllOf(string) method is only used for Unit Tests. Consider removing.
        public IEnumerable<ICalendarProperty> AllOf(string name)
        {
            return _properties.AllOf(name);
        }

        public IEnumerator<ICalendarProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CalendarPropertyList_ItemAdded(object sender, ItemAddedEventArgs<ICalendarProperty> e)
        {
            e.Item.Parent = _parent;
        }
    }
}