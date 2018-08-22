using System.Diagnostics;
using System.Runtime.Serialization;

namespace Ical.Net.Components
{
    // TODO: Consider converting CalendarComponent class to abstract. 

    /// <summary>
    /// This class is used by the parsing framework for iCalendar components.
    /// Generally, you should not need to use this class directly.
    /// </summary>
    [DebuggerDisplay("Component: {Name}")]
    public class CalendarComponent : CalendarObject, ICalendarComponent
    {
        public CalendarComponent(string name) : base(name)
        {
            Initialize();
        }

        private void Initialize()
        {
            Properties = new CalendarPropertyList(this);
        }

        /// <summary>
        /// Returns a list of properties that are associated with the iCalendar object.
        /// </summary>
        public CalendarPropertyList Properties { get; protected set; }

        /// <summary>
        /// Adds a property to this component.
        /// </summary>
        public void AddProperty(string name, string value)
        {
            AddProperty(new CalendarProperty(name, value));
        }

        /// <summary>
        /// Adds a property to this component.
        /// </summary>
        public void AddProperty(ICalendarProperty property)
        {
            property.Parent = this;
            Properties.Set(property.Name, property.Value);
        }

        public override void CopyFrom(ICopyable copyable)
        {
            base.CopyFrom(copyable);

            var component = copyable as ICalendarComponent;
            if (component == null) { return; }

            CopyFrom(component);
        }

        private void CopyFrom(ICalendarComponent component)
        {
            Properties.Clear();
            foreach (var prop in component.Properties)
            {
                Properties.Add(prop);
            }
        }

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }
    }
}