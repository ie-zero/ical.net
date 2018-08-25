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
        private CalendarParameterValue _parameterValue;

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
            _parameterValue = new CalendarParameterValue();
        }

        public string Value
        {
            get { return Values?.FirstOrDefault(); }
        }

        public IEnumerable<string> Values
        {
            get { return _parameterValue.Values; }
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var parameter = copyable as CalendarParameter;
            if (parameter == null) { return; }

            CopyFrom(parameter);
        }

        private void CopyFrom(CalendarParameter parameter)
        {
            _parameterValue = new CalendarParameterValue(parameter.Values);
        }

        public bool ContainsValue(string value)
        {
            return _parameterValue.ContainsValue(value);
        }

        public void SetValue(string value)
        {
            _parameterValue.SetValue(value);
        }

        public void SetValue(IEnumerable<string> values)
        {
            _parameterValue.SetValue(values);
        }

        public void AddValue(string value)
        {
            _parameterValue.AddValue(value);
        }

        public void RemoveValue(string value)
        {
            _parameterValue.RemoveValue(value);
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }
    }
}