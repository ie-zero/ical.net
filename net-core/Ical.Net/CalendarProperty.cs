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
        private readonly CalendarPropertyValue _propertyValue;

        // Required by GroupedValueList class.
        public CalendarProperty() 
        {
            _propertyValue = new CalendarPropertyValue();
        }

        public CalendarProperty(string name) : base(name) 
        {
            _propertyValue = new CalendarPropertyValue();
        }

        public CalendarProperty(string name, object value) : base(name)
        {
            _propertyValue = new CalendarPropertyValue();
            if (value != null) { _propertyValue.AddValue(value); }
        }

        /// <summary>
        /// Returns a list of parameters that are associated with the iCalendar object.
        /// </summary>
        public IParameterCollection Parameters { get; } = new ParameterCollection();

        public object Value
        {
            get { return _propertyValue.Value; }
        }

        public IEnumerable<object> Values
        {
            get { return _propertyValue.Values; }
        }

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
            return _propertyValue.ContainsValue(value);
        }

        public void SetValue(object value)
        {
            _propertyValue.SetValue(value);
        }

        public void SetValue(IEnumerable<object> values)
        {
            _propertyValue.SetValue(values);
        }

        public void AddValue(object value)
        {
            _propertyValue.AddValue(value);
        }

        public void RemoveValue(object value)
        {
            _propertyValue.RemoveValue(value);
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var property = copyable as ICalendarProperty;
            if (property == null) { return; }

            CopyFrom(property);
        }

        private void CopyFrom(ICalendarProperty property)
        {
            SetValue(property.Values);
        }
    }
}