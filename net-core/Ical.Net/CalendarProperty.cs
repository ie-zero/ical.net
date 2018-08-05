using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ical.Net
{
    /// <summary>
    /// A class that represents a property of the <see cref="Calendar"/>
    /// itself or one of its components.  It can also represent non-standard
    /// (X-) properties of an iCalendar component, as seen with many
    /// applications, such as with Apple's iCal.
    /// X-WR-CALNAME:US Holidays
    /// </summary>
    /// <remarks>
    /// Currently, the "known" properties for an iCalendar are as
    /// follows:
    /// <list type="bullet">
    ///     <item>ProdID</item>
    ///     <item>Version</item>
    ///     <item>CalScale</item>
    ///     <item>Method</item>
    /// </list>
    /// There may be other, custom X-properties applied to the calendar,
    /// and X-properties may be applied to calendar components.
    /// </remarks>
    [DebuggerDisplay("{Name}:{Value}")]
    public class CalendarProperty : CalendarObject, ICalendarProperty
    {
        private List<object> _values = new List<object>();

        public CalendarProperty() { }

        public CalendarProperty(string name) : base(name) { }

        public CalendarProperty(string name, object value) : base(name)
        {
            _values.Add(value);
        }

        /// <summary>
        /// Returns a list of parameters that are associated with the iCalendar object.
        /// </summary>
        public IParameterCollection Parameters { get; protected set; } = new ParameterList();

        public object Value
        {
            get { return _values?.FirstOrDefault(); }

            //set
            //{
            //    if (value == null)
            //    {
            //        _values = null;
            //        return;
            //    }

            //    if (_values != null && _values.Count > 0)
            //    {
            //        _values[0] = value;
            //    }
            //    else
            //    {
            //        _values?.Clear();
            //        _values?.Add(value);
            //    }
            //}
        }

        public IEnumerable<object> Values => _values?.AsReadOnly();

        /// <summary>
        /// Adds a parameter to the iCalendar object.
        /// </summary>
        public void AddParameter(string name, string value)
        {
            Parameters.Add(new CalendarParameter(name, value));
        }

        /// <summary>
        /// Adds a parameter to the iCalendar object.
        /// </summary>
        public void AddParameter(CalendarParameter parameter)
        {
            Parameters.Add(parameter);
        }

        public bool ContainsValue(object value)
        {
            return _values.Contains(value);
        }

        public void SetValue(object value)
        {
            if (_values.Count == 0)
            {
                _values.Add(value);
            }
            else if (value != null)
            {
                // Our list contains values.  Let's set the first value!
                _values[0] = value;
            }
            else
            {
                _values.Clear();
            }
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

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var property = copyable as ICalendarProperty;
            if (property == null) { return; }

            SetValue(property.Values);
        }
    }
}