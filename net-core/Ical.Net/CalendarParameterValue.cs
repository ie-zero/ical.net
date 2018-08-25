using System;
using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections.Interfaces;

namespace Ical.Net
{
    public class CalendarParameterValue : IValueObject<string>
    {
        private readonly HashSet<string> _values;

        public CalendarParameterValue()
        {
            _values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public CalendarParameterValue(string value)
        {
            _values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            AddValue(value);
        }

        public CalendarParameterValue(IEnumerable<string> values)
        {
            _values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in values)
            {
                AddValue(value);
            }
        }

        public string Value
        {
            get { return Values?.FirstOrDefault(); }
        }

        public IEnumerable<string> Values
        {
            get { return _values?.ToList()?.AsReadOnly(); }
        }

        public bool ContainsValue(string value)
        {
            return _values.Contains(value);
        }

        public void SetValue(string value)
        {
            _values.Clear();
            _values.Add(value);
        }

        public void SetValue(IEnumerable<string> values)
        {
            _values.Clear();
            _values.UnionWith(values.Where(IsValid));
        }

        public void AddValue(string value)
        {
            if (IsValid(value)) { _values.Add(value); }
        }

        public void RemoveValue(string value)
        {
            _values.Remove(value);
        }

        private bool IsValid(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}