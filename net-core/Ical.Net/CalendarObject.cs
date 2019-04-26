using System;
using System.Runtime.Serialization;
using Ical.Net.Collections;

namespace Ical.Net
{
    /// <summary>
    /// The base class for all iCalendar objects and components.
    /// </summary>
    public abstract class CalendarObject : CalendarObjectBase, ICalendarObject
    {
        private ICalendarObjectList<ICalendarObject> _children;

        private TypedServicesProvider _typedServices;
        private NamedServicesProvider _namedServices;

        internal CalendarObject()
        {
            Initialize();
        }

        public CalendarObject(string name) : this()
        {
            Name = name;
        }

        private void Initialize()
        {
            //ToDo: I'm fairly certain this is ONLY used for null checking. If so, maybe it can just be a bool? CalendarObjectList is an empty object, and
            //ToDo: its constructor parameter is ignored
            _children = new CalendarObjectList(this);

            _typedServices = new TypedServicesProvider();
            _namedServices = new NamedServicesProvider();

            _children.ItemAdded += Children_ItemAdded;
        }

        [OnDeserializing]
        internal void DeserializingInternal(StreamingContext context) => OnDeserializing(context);

        [OnDeserialized]
        internal void DeserializedInternal(StreamingContext context) => OnDeserialized(context);

        protected virtual void OnDeserializing(StreamingContext context) => Initialize();

        protected virtual void OnDeserialized(StreamingContext context) {}

        private void Children_ItemAdded(object sender, ItemAddedEventArgs<ICalendarObject> e) => e.Item.Parent = this;

        protected bool Equals(CalendarObject other) => string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CalendarObject) obj);
        }

        public override int GetHashCode() => Name?.GetHashCode() ?? 0;

        public override void CopyFrom(ICopyable c)
        {
            var obj = c as ICalendarObject;
            if (obj == null)
            {
                return;
            }

            // Copy the name and basic information
            Name = obj.Name;
            Parent = obj.Parent;

            // Add each child
            Children.Clear();
            foreach (var child in obj.Children)
            {
                this.AddChild(child);
            }
        }

        /// <summary>
        /// Returns the parent iCalObject that owns this one.
        /// </summary>
        public virtual ICalendarObject Parent { get; set; }

        /// <summary>
        /// A collection of iCalObjects that are children of the current object.
        /// </summary>
        public virtual ICalendarObjectList<ICalendarObject> Children => _children;

        /// <summary>
        /// Gets or sets the name of the iCalObject.  For iCalendar components, this is the RFC 5545 name of the component.
        /// </summary>        
        public virtual string Name { get; set; }

        /// <summary>
        /// Returns the <see cref="Calendar"/> that this DDayiCalObject belongs to.
        /// </summary>
        public virtual Calendar Calendar
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
            protected set { }
        }

        public virtual object GetService(Type serviceType)
        {
            return _typedServices.GetService(serviceType);
        }

        public virtual object GetService(string name)
        {
            return _namedServices.GetService(name);
        }

        public virtual T GetService<T>()
        {
            return _typedServices.GetService<T>();
        }

        public virtual T GetService<T>(string name)
        {
            return _namedServices.GetService<T>(name);
        }

        public virtual void SetService(string name, object obj)
        {
            _namedServices.SetService(name, obj);
        }

        public virtual void SetService(object obj)
        {
            _typedServices.SetService(obj);
        }

        public virtual void RemoveService(Type type)
        {
            _typedServices.RemoveService(type);
        }

        public virtual void RemoveService(string name)
        {
            _namedServices.RemoveService(name);
        }

        public virtual string Group
        {
            get => Name;
            set => Name = value;
        }
    }
}