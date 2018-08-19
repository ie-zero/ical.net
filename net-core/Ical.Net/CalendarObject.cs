using System;
using System.Runtime.Serialization;
using Ical.Net.Collections;
using Ical.Net.Extensions;

namespace Ical.Net
{
    /// <summary>
    /// The base class for all iCalendar objects and components.
    /// </summary>
    public abstract class CalendarObject : ICalendarObject
    {
        private ServiceProvider _serviceProvider;

        public CalendarObject(string name) : this()
        {
            Name = name;
        }

        internal CalendarObject()
        {
            IsLoaded = true;
            Initialize();
        }

        private void Initialize()
        {
            // TODO: I'm fairly certain this is ONLY used for null checking. 
            //      If so, maybe it can just be a bool? CalendarObjectList is an empty object, and its constructor 
            //      parameter is ignored.
            Children = new CalendarObjectList();
            _serviceProvider = new ServiceProvider();

            Children.ItemAdded += Children_ItemAdded;
        }

        /// <summary>
        /// Returns the <see cref="Calendar"/> that this DDayiCalObject belongs to.
        /// </summary>
        public Calendar Calendar
        {
            get
            {
                ICalendarObject obj = this;
                while (!(obj is Calendar) && obj.Parent != null)
                {
                    obj = obj.Parent;
                }

                return obj as Calendar;
            }
        }

        public string Group
        {
            get { return Name; }
            set { Name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the iCalObject. For iCalendar components, this is the RFC 5545
        /// name of the component.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// The parent calendar object.
        /// </summary>
        public ICalendarObject Parent { get; set; }

        /// <summary>
        /// A collection of calendar object that are children of the current object.
        /// </summary>
        public ICalendarObjectList<ICalendarObject> Children { get; private set; }

        public bool IsLoaded { get; }

        /// <summary>
        /// Copies values from the target object to the current object.
        /// </summary>
        public virtual void CopyFrom(ICopyable copyable)
        {
            var calendarObject = copyable as ICalendarObject;
            if (calendarObject == null) return;

            // Copy the name and basic information
            Name = calendarObject.Name;
            Parent = calendarObject.Parent;

            // Add each child
            Children.Clear();
            foreach (var child in calendarObject.Children)
            {
                this.AddChild(child);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CalendarObject)obj);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public object GetService(string name)
        {
            return _serviceProvider.GetService(name);
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public T GetService<T>(string name)
        {
            return _serviceProvider.GetService<T>(name);
        }

        public void RemoveService(Type type)
        {
            _serviceProvider.RemoveService(type);
        }

        public void RemoveService(string name)
        {
            _serviceProvider.RemoveService(name);
        }

        public void SetService(string name, object obj)
        {
            _serviceProvider.SetService(name, obj);
        }

        public void SetService(object obj)
        {
            _serviceProvider.SetService(obj);
        }

        [OnDeserialized]
        internal void DeserializedInternal(StreamingContext context)
        {
            OnDeserialized(context);
        }

        [OnDeserializing]
        internal void DeserializingInternal(StreamingContext context)
        {
            OnDeserializing(context);
        }

        protected bool Equals(CalendarObject other)
        {
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        protected virtual void OnDeserialized(StreamingContext context) { }

        protected virtual void OnDeserializing(StreamingContext context)
        {
            Initialize();
        }

        private void Children_ItemAdded(object sender, ItemProcessedEventArgs<ICalendarObject> e)
        {
            e.Item.Parent = this;
        }
    }
}