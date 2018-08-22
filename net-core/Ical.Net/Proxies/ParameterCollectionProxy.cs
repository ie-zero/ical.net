using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;
using Ical.Net.Collections.Proxies;

namespace Ical.Net.Proxies
{
    public class ParameterCollectionProxy : IParameterCollection
    {
        private GroupedCollectionProxy<CalendarParameter, CalendarParameter> _proxy;

        public ParameterCollectionProxy(IGroupedCollection<CalendarParameter> realObject)
        {
            _proxy = new GroupedCollectionProxy<CalendarParameter, CalendarParameter>(realObject);
        }

        public event EventHandler<ItemProcessedEventArgs<CalendarParameter>> ItemAdded;

        public int Count => throw new NotImplementedException();

        private GroupedValueList<CalendarParameter, string> Parameters
        {
            get { return _proxy.RealObject as GroupedValueList<CalendarParameter, string>; }
        }

        public void Add(string name, string value)
        {
            _proxy.RealObject.Add(new CalendarParameter(name, value));
        }

        public void Add(CalendarParameter item)
        {
            _proxy.Add(item);
        }

        public IEnumerable<CalendarParameter> AllOf(string group)
        {
            throw new NotImplementedException();
        }

        public void Clear(string group)
        {
            throw new NotImplementedException();
        }

        public bool Contains(CalendarParameter item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string group)
        {
            return _proxy.ContainsKey(group);
        }

        public int CountOf(string group)
        {
            throw new NotImplementedException();
        }

        public string Get(string name)
        {
            var parameter = _proxy.RealObject.FirstOrDefault(o => string.Equals(o.Name, name, StringComparison.Ordinal));

            return parameter?.Value;
        }

        public IEnumerator<CalendarParameter> GetEnumerator()
        {
            return _proxy.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _proxy.GetEnumerator();
        }

        public IList<string> GetMany(string name)
        {
            return new GroupedValueListProxy<CalendarParameter, string, string>(Parameters, name);
        }

        // TODO: Consider removing method as obsolete.
        public int IndexOf(CalendarParameter obj)
        {
            throw new NotImplementedException();
        }

        // TODO: Consider removing method as obsolete.
        public void Insert(int index, CalendarParameter item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string group)
        {
            return _proxy.Remove(group);
        }

        public bool Remove(CalendarParameter item)
        {
            return _proxy.Remove(item);
        }

        // TODO: Consider removing method as obsolete.
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Set(string name, string value)
        {
            var parameter = _proxy.RealObject.FirstOrDefault(o => string.Equals(o.Name, name, StringComparison.Ordinal));

            if (parameter == null)
            {
                _proxy.RealObject.Add(new CalendarParameter(name, value));
            }
            else
            {
                parameter.SetValue(value);
            }
        }

        public void Set(string name, IEnumerable<string> values)
        {
            var parameter = _proxy.RealObject.FirstOrDefault(o => string.Equals(o.Name, name, StringComparison.Ordinal));

            if (parameter == null)
            {
                _proxy.RealObject.Add(new CalendarParameter(name, values));
            }
            else
            {
                parameter.SetValue(values);
            }
        }

        public void SetParent(ICalendarObject parent)
        {
            foreach (var parameter in this)
            {
                parameter.Parent = parent;
            }
        }

        public void SetProxiedObject(IGroupedCollection<CalendarParameter> realObject)
        {
            _proxy = new GroupedCollectionProxy<CalendarParameter, CalendarParameter>(realObject);
        }
    }
}