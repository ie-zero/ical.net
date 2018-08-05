using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Ical.Net.Collections.Interfaces;

namespace Ical.Net
{
    [DebuggerDisplay("{Name}={string.Join(\",\", Values)}")]
    public class CalendarParameter : CalendarObject, IValueObject<string>
    {
        private HashSet<string> _values;

        public CalendarParameter()
        {
            Initialize();
        }

        public CalendarParameter(string name) : base(name)
        {
            Initialize();
        }

        public CalendarParameter(string name, string value) : base(name)
        {
            Initialize();
            AddValue(value);
        }

        public CalendarParameter(string name, IEnumerable<string> values) : base(name)
        {
            Initialize();
            foreach (var v in values)
            {
                AddValue(v);
            }
        }

        private void Initialize()
        {
            _values = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Value
        {
            get { return Values?.FirstOrDefault(); }
            set { SetValue(value); }
        }

        public IEnumerable<string> Values => _values.ToList().AsReadOnly();

        public void AddValue(string value)
        {
            if (IsValidValue(value)){ _values.Add(value); }
        }

        public bool ContainsValue(string value)
        {
            return _values.Contains(value);
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var p = copyable as CalendarParameter;
            if (p?.Values == null)
            {
                return;
            }

            _values = new HashSet<string>(p.Values.Where(IsValidValue), StringComparer.OrdinalIgnoreCase);
        }

        public void SetValue(string value)
        {
            _values.Add(value);
        }

        public void SetValue(IEnumerable<string> values)
        {
            // Remove all previous values
            _values.Clear();
            _values.UnionWith(values.Where(IsValidValue));
        }

        public void RemoveValue(string value)
        {
            _values.Remove(value);
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }

        private bool IsValidValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}