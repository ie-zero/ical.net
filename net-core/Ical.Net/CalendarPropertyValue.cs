using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections.Interfaces;

namespace Ical.Net
{
    public class CalendarPropertyValue : IValueObject<object>
    {
        // TODO: Verify that duplicates values are allowed in Property value.
        private readonly List<object> _values;

        public CalendarPropertyValue()
        {
            _values = new List<object>();
        }

        public CalendarPropertyValue(string value)
        {
            _values = new List<object>();
            AddValue(value);
        }

        public CalendarPropertyValue(IEnumerable<object> values)
        {
            _values = new List<object>();
            foreach (var value in values)
            {
                AddValue(value);
            }
        }

        public object Value
        {
            get { return _values?.FirstOrDefault(); }
        }

        public IEnumerable<object> Values
        {
            get { return _values?.AsReadOnly(); }
        }

        public bool ContainsValue(object value)
        {
            return _values.Contains(value);
        }

        public void SetValue(object value)
        {
            _values.Clear();
            if (value != null) { _values.Add(value); }
        }

        public void SetValue(IEnumerable<object> values)
        {
            _values.Clear();
            _values.AddRange(values ?? Enumerable.Empty<object>());
        }

        public void AddValue(object value)
        {
            if (value == null) { return; }
            _values.Add(value);
        }

        public void RemoveValue(object value)
        {
            if (value == null) { return; }
            _values.Remove(value);
        }
    }
}